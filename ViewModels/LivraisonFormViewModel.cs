using Microsoft.AspNetCore.Mvc.Rendering;
using TMS_Project.Models;

namespace TMS_Project.ViewModels
{
    public class LivraisonFormViewModel
    {
        public Livraison Livraison { get; set; } = new Livraison
        {
            DateLivraison = DateTime.Today,
            StatutLivraison = "En attente"
        };

        public IEnumerable<SelectListItem> Clients { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Tournees { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
