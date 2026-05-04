using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;
using TMS_Project.ViewModels;

namespace TMS_Project.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    public class TourneeController : Controller
    {
        private readonly TmsDbContext _context;

        public TourneeController(TmsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tournees = await _context.Tournees
                .Include(t => t.Transporteur)
                .Include(t => t.Camion)
                .Include(t => t.Chauffeur)
                .Include(t => t.Livraisons)
                .Include(t => t.Couts)
                .OrderByDescending(t => t.DateTournee)
                .ToListAsync();

            return View(tournees);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Create()
        {
            var vm = new TourneeFormViewModel();
            await BuildLists(vm);
            return View(vm);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourneeFormViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    vm.Tournee.MontantEstime = vm.MontantCout > 0 ? vm.MontantCout : 0;
                    vm.Tournee.CoutTotal = vm.MontantCout;

                    _context.Add(vm.Tournee);
                    await _context.SaveChangesAsync();

                    if (vm.SelectedLivraisonIds != null && vm.SelectedLivraisonIds.Any())
                    {
                        var livraisons = await _context.Livraisons
                            .Where(l => vm.SelectedLivraisonIds.Contains(l.LivraisonId))
                            .ToListAsync();

                        foreach (var livraison in livraisons)
                        {
                            livraison.TourneeId = vm.Tournee.TourneeId;
                        }

                        await _context.SaveChangesAsync();
                    }

                    if (vm.MontantCout > 0)
                    {
                        _context.Couts.Add(new Cout
                        {
                            TourneeId = vm.Tournee.TourneeId,
                            TypeCout = vm.TypeCout ?? "Initial",
                            Montant = vm.MontantCout,
                            Description = vm.DescCout,
                            DateCout = DateTime.Now
                        });
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erreur: " + ex.Message);
                }
            }

            await BuildLists(vm);
            return View(vm);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tournee = await _context.Tournees
                .Include(t => t.Livraisons)
                .FirstOrDefaultAsync(m => m.TourneeId == id);

            if (tournee == null) return NotFound();

            var vm = new TourneeFormViewModel
            {
                Tournee = tournee,
                SelectedLivraisonIds = tournee.Livraisons.Select(l => l.LivraisonId).ToList()
            };

            await BuildLists(vm);
            return View(vm);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourneeFormViewModel vm)
        {
            if (id != vm.Tournee.TourneeId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    vm.Tournee.MontantEstime ??= 0;
                    _context.Update(vm.Tournee);

                    if (vm.MontantCout > 0)
                    {
                        _context.Couts.Add(new Cout
                        {
                            TourneeId = id,
                            TypeCout = vm.TypeCout ?? "Additionnel",
                            Montant = vm.MontantCout,
                            Description = vm.DescCout,
                            DateCout = DateTime.Now
                        });
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "La tournee a ete modifiee avec succes.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Verifiez les champs obligatoires.");
                }
            }

            await BuildLists(vm);
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tournee = await _context.Tournees
                .Include(t => t.Transporteur).Include(t => t.Camion).Include(t => t.Chauffeur)
                .Include(t => t.Couts).Include(t => t.Livraisons).ThenInclude(l => l.Client)
                .FirstOrDefaultAsync(m => m.TourneeId == id);

            return tournee == null
                ? NotFound()
                : View(new TourneeDetailsViewModel { Tournee = tournee, Couts = tournee.Couts.ToList() });
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tournee = await _context.Tournees
                .Include(t => t.Transporteur)
                .Include(t => t.Camion)
                .Include(t => t.Chauffeur)
                .FirstOrDefaultAsync(m => m.TourneeId == id);

            return tournee == null ? NotFound() : View(tournee);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournee = await _context.Tournees.FindAsync(id);
            if (tournee != null)
            {
                _context.Tournees.Remove(tournee);
                await _context.SaveChangesAsync();
                TempData["Success"] = "La tournee a ete supprimee avec succes.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task BuildLists(TourneeFormViewModel vm)
        {
            vm.Transporteurs = await _context.Transporteurs
                .Select(t => new SelectListItem { Value = t.TransporteurId.ToString(), Text = t.Nom })
                .ToListAsync();

            vm.Camions = await _context.Camions
                .Select(c => new SelectListItem { Value = c.CamionId.ToString(), Text = c.Immatriculation })
                .ToListAsync();

            vm.Chauffeurs = await _context.Chauffeurs
                .Select(ch => new SelectListItem { Value = ch.ChauffeurId.ToString(), Text = ch.Nom })
                .ToListAsync();

            vm.LivraisonsDisponibles = await _context.Livraisons
                .Where(l => l.TourneeId == null || (vm.Tournee != null && l.TourneeId == vm.Tournee.TourneeId))
                .ToListAsync();
        }
    }
}
