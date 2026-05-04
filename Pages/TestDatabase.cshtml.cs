using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Pages
{
    public class TestDatabaseModel : PageModel
    {
        private readonly TmsDbContext _context;

        public bool IsConnected { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public int TransporteurCount { get; set; }
        public int CamionCount { get; set; }
        public int ChauffeurCount { get; set; }
        public int ClientCount { get; set; }
        public int TourneeCount { get; set; }
        public int LivraisonCount { get; set; }
        public int CoutCount { get; set; }

        public List<Transporteur> Transporteurs { get; set; } = new();
        public List<Tournee> Tournees { get; set; } = new();

        public TestDatabaseModel(TmsDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            try
            {
                // Test database connection
                bool canConnect = await _context.Database.CanConnectAsync();

                if (!canConnect)
                {
                    IsConnected = false;
                    ErrorMessage = "Cannot connect to database. Make sure SQL Server is running and the database exists.";
                    return;
                }

                // Get counts
                TransporteurCount = await _context.Transporteurs.CountAsync();
                CamionCount = await _context.Camions.CountAsync();
                ChauffeurCount = await _context.Chauffeurs.CountAsync();
                ClientCount = await _context.Clients.CountAsync();
                TourneeCount = await _context.Tournees.CountAsync();
                LivraisonCount = await _context.Livraisons.CountAsync();
                CoutCount = await _context.Couts.CountAsync();

                // Get sample data
                Transporteurs = await _context.Transporteurs.Take(3).ToListAsync();
                Tournees = await _context.Tournees
                    .Include(t => t.Chauffeur)
                    .Include(t => t.Camion)
                    .Take(5)
                    .ToListAsync();

                IsConnected = true;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                ErrorMessage = $"Error: {ex.Message}";
            }
        }
    }
}
