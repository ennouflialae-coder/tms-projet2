using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    public class CamionController : Controller
    {
        private readonly TmsDbContext _context;

        public CamionController(TmsDbContext context)
        {
            _context = context;
        }

        // L-Index
        public async Task<IActionResult> Index()
        {
            var camions = await _context.Camions
                .Include(c => c.Transporteur)
                .OrderBy(c => c.Immatriculation)
                .ToListAsync();
            return View(camions);
        }

        // Edit (GET)
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var camion = await _context.Camions.FindAsync(id);
            if (camion == null) return NotFound();

            await PopulateTransporteursAsync(camion.TransporteurId);
            return View(camion);
        }

        // Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Edit(int id, Camion camion)
        {
            if (id != camion.CamionId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camion);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Le camion a été modifié avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erreur: " + ex.Message);
                }
            }
            await PopulateTransporteursAsync(camion.TransporteurId);
            return View(camion);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var camion = await _context.Camions.FindAsync(id);
            if (camion == null) return NotFound();

            try
            {
                _context.Camions.Remove(camion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Camion supprimé avec succès !";
            }
            catch (Exception) 
            {
                TempData["Error"] = "Impossible de supprimer ce camion car il est lié à une ou plusieurs tournées.";
            }

            return RedirectToAction(nameof(Index));
        }
        // Helper 
        private async Task PopulateTransporteursAsync(int? selectedId = null)
        {
            var transporteurs = await _context.Transporteurs.OrderBy(t => t.Nom).ToListAsync();
            ViewBag.TransporteurId = new SelectList(transporteurs, "TransporteurId", "Nom", selectedId);
        }

        public async Task<IActionResult> Details(int id)
        {
            var camion = await _context.Camions
                .Include(c => c.Transporteur).Include(c => c.Tournees)
                .FirstOrDefaultAsync(c => c.CamionId == id);
            return camion == null ? NotFound() : View(camion);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Create()
        {
            await PopulateTransporteursAsync();
            return View(new Camion { DateAcquisition = DateTime.Today, EstActif = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Create(Camion camion)
        {
            if (ModelState.IsValid)
            {
                _context.Camions.Add(camion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Camion créé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateTransporteursAsync(camion.TransporteurId);
            return View(camion);
        }
    }
}
