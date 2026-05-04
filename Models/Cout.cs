namespace TMS_Project.Models
{
    public class Cout
    {
        public int CoutId { get; set; }
        public int TourneeId { get; set; }
        public string TypeCout { get; set; } = string.Empty; // Carburant, Péage, Maintenance, Salaire
        public decimal Montant { get; set; }
        public DateTime DateCout { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;

        // Foreign Key
        public Tournee Tournee { get; set; } = null!;
    }
}
