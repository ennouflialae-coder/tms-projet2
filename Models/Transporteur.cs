namespace TMS_Project.Models
{
    public class Transporteur
    {
        public int TransporteurId { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; } = DateTime.Now;

        // Relationships
        public ICollection<Camion> Camions { get; set; } = new List<Camion>();
        public ICollection<Chauffeur> Chauffeurs { get; set; } = new List<Chauffeur>();
        public ICollection<Tournee> Tournees { get; set; } = new List<Tournee>();
    }
}
