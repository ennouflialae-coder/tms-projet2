using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS_Project.Models
{
    [Table("Livraisons")]
    public class Livraison
    {
        [Key]
        public int LivraisonId { get; set; }
        public int? TourneeId { get; set; }

        [ForeignKey("TourneeId")]
        public virtual Tournee? Tournee { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        public string Adresse { get; set; } = string.Empty;
        public int PoidsKg { get; set; }
        public DateTime DateLivraison { get; set; }
        public string StatutLivraison { get; set; } = "En attente";

        [Column(TypeName = "decimal(18,2)")]
        public decimal MontantEstime { get; set; }

        public DateTime? DateRealisation { get; set; }

        [NotMapped]
        public decimal CoutTotal { get; set; }
    }
}