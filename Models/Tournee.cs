namespace TMS_Project.Models
{
    public class Tournee
    {
        public int TourneeId { get; set; }
        public DateTime DateTournee { get; set; }
        public string StatutTournee { get; set; } = "Planifiée";
        public decimal CoutTotal { get; set; }

        // Foreign Keys
        public int TransporteurId { get; set; }
        public int CamionId { get; set; }
        public int ?ChauffeurId { get; set; }

        // Navigation Properties (DAROURI d-dir had '?' hna)
        public virtual Transporteur? Transporteur { get; set; }
        public virtual Camion? Camion { get; set; }
        public virtual Chauffeur? Chauffeur { get; set; }

        public virtual ICollection<Livraison> Livraisons { get; set; } = new List<Livraison>();
        public virtual ICollection<Cout> Couts { get; set; } = new List<Cout>();
        // F Models/Tournee.cs
        public decimal? MontantEstime { get; set; } 
    }
}