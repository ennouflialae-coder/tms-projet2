namespace TMS_Project.Models
{
    public class Chauffeur
    {
        public int ChauffeurId { get; set; }
        public int TransporteurId { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NumeroPermis { get; set; } = string.Empty;
        public DateTime DateEmbauche { get; set; }
        public bool EstActif { get; set; } = true;

        // Foreign Key
        public virtual Transporteur? Transporteur { get; set; }

        // Relationships
        public ICollection<Tournee> Tournees { get; set; } = new List<Tournee>();
    }
}
