namespace TMS_Project.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public ICollection<Livraison> Livraisons { get; set; } = new List<Livraison>();
        public ICollection<DemandeLivraison> DemandesLivraison { get; set; } = new List<DemandeLivraison>();
    }
}
