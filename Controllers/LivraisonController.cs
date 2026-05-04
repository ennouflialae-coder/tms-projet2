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
    public class LivraisonController : Controller
    {
        private readonly TmsDbContext _context;

        public LivraisonController(TmsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livraisons = await _context.Livraisons
                .Include(l => l.Client)
                .Include(l => l.Tournee)
                .OrderByDescending(l => l.DateLivraison)
                .ToListAsync();

            return View(livraisons);
        }

        public async Task<IActionResult> Details(int id)
        {
            var livraison = await _context.Livraisons
                .Include(l => l.Client)
                .Include(l => l.Tournee!).ThenInclude(t => t.Camion)
                .Include(l => l.Tournee!).ThenInclude(t => t.Chauffeur)
                .FirstOrDefaultAsync(l => l.LivraisonId == id);

            return livraison == null ? NotFound() : View(livraison);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Create(int? tourneeId)
        {
            var vm = await BuildFormViewModelAsync();
            if (tourneeId.HasValue)
            {
                vm.Livraison.TourneeId = tourneeId.Value;
            }

            return View(vm);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LivraisonFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var rebuilt = await BuildFormViewModelAsync();
                rebuilt.Livraison = vm.Livraison;
                return View(rebuilt);
            }

            _context.Livraisons.Add(vm.Livraison);
            await _context.SaveChangesAsync();
            await RecalculerCoutTournee(vm.Livraison.TourneeId);

            TempData["Success"] = $"Livraison #{vm.Livraison.LivraisonId} creee avec succes.";

            if (vm.Livraison.TourneeId.HasValue && vm.Livraison.TourneeId.Value > 0)
            {
                return RedirectToAction("Details", "Tournee", new { id = vm.Livraison.TourneeId.Value });
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var livraison = await _context.Livraisons.FindAsync(id);
            if (livraison == null) return NotFound();

            var vm = await BuildFormViewModelAsync();
            vm.Livraison = livraison;
            return View(vm);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LivraisonFormViewModel vm)
        {
            if (id != vm.Livraison.LivraisonId) return NotFound();

            if (!ModelState.IsValid)
            {
                var rebuilt = await BuildFormViewModelAsync();
                rebuilt.Livraison = vm.Livraison;
                return View(rebuilt);
            }

            if (vm.Livraison.StatutLivraison == "Livree" && vm.Livraison.DateRealisation == null)
            {
                vm.Livraison.DateRealisation = DateTime.Now;
            }

            _context.Attach(vm.Livraison).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await RecalculerCoutTournee(vm.Livraison.TourneeId);

            TempData["Success"] = $"Livraison #{id} modifiee avec succes.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livraison = await _context.Livraisons.FindAsync(id);
            if (livraison != null)
            {
                int? tourneeId = livraison.TourneeId;
                _context.Livraisons.Remove(livraison);
                await _context.SaveChangesAsync();

                await RecalculerCoutTournee(tourneeId);
                TempData["Success"] = $"Livraison #{id} supprimee.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<LivraisonFormViewModel> BuildFormViewModelAsync()
        {
            var clients = await _context.Clients.OrderBy(c => c.Nom).ToListAsync();
            var tournees = await _context.Tournees
                .Where(t => t.StatutTournee != "Terminee")
                .OrderByDescending(t => t.DateTournee)
                .ToListAsync();

            return new LivraisonFormViewModel
            {
                Clients = clients.Select(c => new SelectListItem { Value = c.ClientId.ToString(), Text = c.Nom }),
                Tournees = tournees.Select(t => new SelectListItem { Value = t.TourneeId.ToString(), Text = $"Tournee #{t.TourneeId} - {t.DateTournee:dd/MM/yyyy}" })
            };
        }

        private async Task RecalculerCoutTournee(int? tourneeId)
        {
            if (!tourneeId.HasValue || tourneeId <= 0) return;

            var tournee = await _context.Tournees
                .Include(t => t.Couts)
                .FirstOrDefaultAsync(t => t.TourneeId == tourneeId.Value);

            if (tournee != null)
            {
                tournee.CoutTotal = tournee.Couts.Sum(c => c.Montant);
                await _context.SaveChangesAsync();
            }
        }
    }
}
