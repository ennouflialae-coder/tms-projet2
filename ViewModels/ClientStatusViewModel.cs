using System.ComponentModel.DataAnnotations;
using TMS_Project.Models;

namespace TMS_Project.ViewModels
{
    public class ClientStatusViewModel
    {
        [Required(ErrorMessage = "La reference est obligatoire.")]
        [Display(Name = "Reference de demande")]
        public int? DemandeLivraisonId { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Adresse email invalide.")]
        [Display(Name = "Email utilise dans la demande")]
        public string EmailClient { get; set; } = string.Empty;

        public DemandeLivraison? Demande { get; set; }
    }
}
