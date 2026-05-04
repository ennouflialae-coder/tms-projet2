using System.ComponentModel.DataAnnotations;

namespace TMS_Project.ViewModels
{
    public class ClientDemandeViewModel
    {
        [Required]
        [Display(Name = "Nom complet")]
        public string NomClient { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailClient { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Téléphone")]
        public string TelephoneClient { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Adresse du client")]
        public string AdresseClient { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Type de service")]
        public string TypeService { get; set; } = "Standard";

        [Required]
        [Display(Name = "Description de la marchandise")]
        public string DescriptionMarchandise { get; set; } = string.Empty;

        [Range(1, 10000)]
        [Display(Name = "Quantité")]
        public int Quantite { get; set; } = 1;

        [Required]
        [Display(Name = "Adresse de livraison")]
        public string AdresseLivraison { get; set; } = string.Empty;

        [Range(1, 50000)]
        [Display(Name = "Poids (kg)")]
        public int PoidsKg { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date souhaitée")]
        public DateTime DateSouhaitee { get; set; } = DateTime.Today.AddDays(1);
    }
}
