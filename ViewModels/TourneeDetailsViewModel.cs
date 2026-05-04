using TMS_Project.Models;

namespace TMS_Project.ViewModels
{
    public class TourneeDetailsViewModel
    {
        public Tournee Tournee { get; set; } = new Tournee();
        public List<Cout> Couts { get; set; } = new();
    }
}
