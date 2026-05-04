using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    public class TransporteurController : Controller
    {
        private readonly TmsDbContext _context;

        public TransporteurController(TmsDbContext context)
        {
            _context = context;
        }

        // GET: Transporteur
        public async Task<IActionResult> Index()
        {
            var transporteurs = await _context.Transporteurs
                .Include(t => t.Camions)
                .Include(t => t.Chauffeurs)
                .OrderBy(t => t.Nom)
                .ToListAsync();

            return View(transporteurs);
        }

        // GET: Transporteur/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var transporteur = await _context.Transporteurs
                .Include(t => t.Camions)
                .Include(t => t.Chauffeurs)
                .Include(t => t.Tournees)
                .FirstOrDefaultAsync(t => t.TransporteurId == id);

            if (transporteur == null) return NotFound();
            return View(transporteur);
        }

        // GET: Transporteur/Create
        [Authorize(Roles = ApplicationRoles.Admin)]
        public IActionResult Create()
        {
            return View(new Transporteur
            {
                DateCreation = DateTime.Today
            });
        }

        // POST: Transporteur/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Create(Transporteur transporteur)
        {
            if (!ModelState.IsValid)
            {
                return View(transporteur);
            }

            _context.Transporteurs.Add(transporteur);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Transporteur {transporteur.Nom} créé avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Transporteur/Edit/5
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var transporteur = await _context.Transporteurs.FindAsync(id);
            if (transporteur == null) return NotFound();

            return View(transporteur);
        }

        // POST: Transporteur/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Edit(int id, [Bind("TransporteurId,Nom,Adresse,Telephone,Email,DateCreation")] Transporteur transporteur)
        {
            if (id != transporteur.TransporteurId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transporteur);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Le transporteur {transporteur.Nom} a été mis à jour avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransporteurExists(transporteur.TransporteurId)) return NotFound();
                    else throw;
                }
            }
            return View(transporteur);
        }

        // POST: Transporteur/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var transporteur = await _context.Transporteurs.FindAsync(id);
            if (transporteur == null) return NotFound();

            try
            {
                _context.Transporteurs.Remove(transporteur);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Transporteur supprimé avec succès.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Action impossible : Ce transporteur possède des camions ou des tournées en cours.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TransporteurExists(int id)
        {
            return _context.Transporteurs.Any(e => e.TransporteurId == id);
        }
    }
}
