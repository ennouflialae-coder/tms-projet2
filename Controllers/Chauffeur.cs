using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    public class ChauffeurController : Controller
    {
        private readonly TmsDbContext _context;

        public ChauffeurController(TmsDbContext context)
        {
            _context = context;
        }

        // --- LISTE DES CHAUFFEURS ---
        public async Task<IActionResult> Index()
        {
            var chauffeurs = await _context.Chauffeurs
                .Include(c => c.Transporteur)
                .OrderBy(c => c.Nom)
                .ThenBy(c => c.Prenom)
                .ToListAsync();

            return View(chauffeurs);
        }

        // --- DÉTAILS ---
        public async Task<IActionResult> Details(int id)
        {
            var chauffeur = await _context.Chauffeurs
                .Include(c => c.Transporteur)
                .Include(c => c.Tournees)
                .FirstOrDefaultAsync(c => c.ChauffeurId == id);

            if (chauffeur == null) return NotFound();
            return View(chauffeur);
        }

        // --- CRÉATION (GET) ---
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Create()
        {
            await PopulateTransporteursAsync();
            return View(new Chauffeur
            {
                DateEmbauche = DateTime.Today,
                EstActif = true
            });
        }

        // --- MODIFICATION (GET) ---
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var chauffeur = await _context.Chauffeurs.FindAsync(id);
            if (chauffeur == null) return NotFound();

            // K-n-sta3mlou l-helper bach n-3emrou l-liste dial les transporteurs
            await PopulateTransporteursAsync(chauffeur.TransporteurId);
            return View(chauffeur);
        }

        // --- MODIFICATION (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Edit(int id, Chauffeur chauffeur)
        {
            if (id != chauffeur.ChauffeurId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chauffeur);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Chauffeur modifié avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Chauffeurs.Any(e => e.ChauffeurId == chauffeur.ChauffeurId)) return NotFound();
                    else throw;
                }
            }


            await PopulateTransporteursAsync(chauffeur.TransporteurId);
            return View(chauffeur);
        }

        // --- SUPPRESSION (POST) ---
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chauffeur = await _context.Chauffeurs
                .Include(c => c.Tournees)
                .FirstOrDefaultAsync(c => c.ChauffeurId == id);

            if (chauffeur == null) return NotFound();

            if (chauffeur.Tournees != null && chauffeur.Tournees.Any())
            {
                TempData["Error"] = "Vous ne pouvez pas supprimer ce chauffeur car il est lié à une ou plusieurs tournées.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Chauffeurs.Remove(chauffeur);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Chauffeur supprimé avec succès.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Une erreur s'est produite lors de la suppression.";
            }

            return RedirectToAction(nameof(Index));
        }

        // --- HELPER METHOD ---
        private async Task PopulateTransporteursAsync(int? selectedId = null)
        {
            var transporteurs = await _context.Transporteurs
                .OrderBy(t => t.Nom)
                .ToListAsync();

            ViewBag.TransporteurId = new SelectList(transporteurs, "TransporteurId", "Nom", selectedId);
        }
    }
}
