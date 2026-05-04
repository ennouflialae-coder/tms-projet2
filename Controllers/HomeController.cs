using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly TmsDbContext _context;

        public HomeController(TmsDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Tarifs()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Services()
        {
            return View();
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.Transporteurs = await _context.Transporteurs.CountAsync();
            ViewBag.CamionsActifs = await _context.Camions.CountAsync(c => c.EstActif);
            ViewBag.ChauffeursActifs = await _context.Chauffeurs.CountAsync(c => c.EstActif);
            ViewBag.LivraisonsJour = await _context.Livraisons.CountAsync(l => l.DateLivraison.Date == DateTime.Today);
            ViewBag.DemandesEnAttente = await _context.DemandesLivraison.CountAsync(d => d.Statut == "En attente");
            ViewBag.TourneesEnCours = await _context.Tournees.CountAsync(t => t.StatutTournee == "En cours");

            return View();
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
