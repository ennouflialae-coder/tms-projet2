using Microsoft.AspNetCore.Mvc.Rendering;
using TMS_Project.Models;

namespace TMS_Project.ViewModels
{
    public class TourneeFormViewModel
    {
        public Tournee Tournee { get; set; } = new Tournee
        {
            DateTournee = DateTime.Today,
            StatutTournee = "Planifiee"
        };

        public List<int> SelectedLivraisonIds { get; set; } = new();
        public string TypeCout { get; set; } = "Carburant";
        public decimal MontantCout { get; set; }
        public string DescCout { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Transporteurs { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Camions { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Chauffeurs { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<Livraison> LivraisonsDisponibles { get; set; } = Enumerable.Empty<Livraison>();
    }
}
