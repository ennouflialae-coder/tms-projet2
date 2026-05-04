using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;
using TMS_Project.ViewModels;

namespace TMS_Project.Controllers
{
    public class DemandeLivraisonController : Controller
    {
        private readonly TmsDbContext _context;

        public DemandeLivraisonController(TmsDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = ApplicationRoles.User)]
        public IActionResult Inscription()
        {
            return View(new ClientDemandeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.User)]
        public async Task<IActionResult> Inscription(ClientDemandeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == vm.EmailClient);
            if (client == null)
            {
                client = new Client
                {
                    Nom = vm.NomClient,
                    Email = vm.EmailClient,
                    Telephone = vm.TelephoneClient,
                    Adresse = vm.AdresseClient,
                    DateCreation = DateTime.Now
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }
            else
            {
                client.Nom = vm.NomClient;
                client.Telephone = vm.TelephoneClient;
                client.Adresse = vm.AdresseClient;
                await _context.SaveChangesAsync();
            }

            var demande = new DemandeLivraison
            {
                ClientId = client.ClientId,
                TypeService = vm.TypeService,
                DescriptionMarchandise = vm.DescriptionMarchandise,
                Quantite = vm.Quantite,
                Adresse = vm.AdresseLivraison,
                PoidsKg = vm.PoidsKg,
                DateSouhaitee = vm.DateSouhaitee,
                Statut = "En attente",
                MontantEstime = CalculerMontant(vm.TypeService, vm.PoidsKg, vm.Quantite),
                CreatedAt = DateTime.Now
            };

            _context.DemandesLivraison.Add(demande);
            await _context.SaveChangesAsync();

            TempData["ClientNotification"] = "Votre demande est en cours d'execution. Notre equipe va la verifier rapidement.";

            return RedirectToAction(nameof(Confirmation), new { id = demande.DemandeLivraisonId });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Confirmation(int id)
        {
            var demande = await _context.DemandesLivraison
                .Include(d => d.Client)
                .FirstOrDefaultAsync(d => d.DemandeLivraisonId == id);

            return demande == null ? NotFound() : View(demande);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Admin()
        {
            var demandes = await _context.DemandesLivraison
                .Include(d => d.Client)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            ViewBag.NouvellesDemandes = demandes.Count(d => d.Statut == "En attente");
            return View(demandes);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Suivi()
        {
            return View(new ClientStatusViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Suivi(ClientStatusViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var demande = await _context.DemandesLivraison
                .Include(d => d.Client)
                .FirstOrDefaultAsync(d =>
                    d.DemandeLivraisonId == vm.DemandeLivraisonId &&
                    d.Client != null &&
                    d.Client.Email == vm.EmailClient);

            if (demande == null)
            {
                ModelState.AddModelError(string.Empty, "Aucune demande trouvee avec cette reference et cet email.");
                return View(vm);
            }

            vm.Demande = demande;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Accepter(int id)
        {
            var demande = await _context.DemandesLivraison
                .Include(d => d.Client)
                .FirstOrDefaultAsync(d => d.DemandeLivraisonId == id);

            if (demande == null) return NotFound();

            demande.Statut = "Acceptee";
            demande.DateValidation = DateTime.Now;
            if (string.IsNullOrWhiteSpace(demande.NumeroRecu))
            {
                demande.NumeroRecu = $"REC-{DateTime.Now:yyyyMMdd}-{demande.DemandeLivraisonId:D4}";
            }
            demande.CommentaireValidation = "Demande validee avec succes.";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> Refuser(int id)
        {
            var demande = await _context.DemandesLivraison.FindAsync(id);
            if (demande == null) return NotFound();

            demande.Statut = "Refusee";
            demande.DateValidation = DateTime.Now;
            demande.CommentaireValidation = "Demande refusee.";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> MarquerLivree(int id)
        {
            var demande = await _context.DemandesLivraison.FindAsync(id);
            if (demande == null) return NotFound();

            demande.Statut = "Livree";
            demande.CommentaireValidation = "Commande livree au client.";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Recu(int id)
        {
            var demande = await _context.DemandesLivraison
                .Include(d => d.Client)
                .FirstOrDefaultAsync(d => d.DemandeLivraisonId == id);

            return demande == null ? NotFound() : View(demande);
        }

        private static decimal CalculerMontant(string typeService, int poidsKg, int quantite)
        {
            decimal basePrice = 120m + (poidsKg * 1.5m) + (quantite * 10m);

            return typeService switch
            {
                "Express" => basePrice * 1.45m,
                "Fragile" => basePrice * 1.25m,
                "Refrigere" => basePrice * 1.60m,
                _ => basePrice
            };
        }
    }
}
