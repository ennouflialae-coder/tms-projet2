namespace TMS_Project.Models
{
    public class DemandeLivraison
    {
        public int DemandeLivraisonId { get; set; }
        public int? ClientId { get; set; }
        public string TypeService { get; set; } = "Standard";
        public string DescriptionMarchandise { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public int Quantite { get; set; } = 1;
        public int PoidsKg { get; set; }
        public DateTime DateSouhaitee { get; set; }
        public string Statut { get; set; } = "En attente";
        public decimal MontantEstime { get; set; }
        public int? CamionAffecteId { get; set; }
        public int? ChauffeurAffecteId { get; set; }
        public string NumeroRecu { get; set; } = string.Empty;
        public DateTime? DateValidation { get; set; }
        public string CommentaireValidation { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Client? Client { get; set; }
    }
}
