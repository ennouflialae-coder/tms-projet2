using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Models;
using TMS_Project.Data;

namespace TMS_Project.Pages.Transporteurs
{
    public class IndexTrasporteursModel : PageModel
    {
 
        private readonly TmsDbContext _context;

        public IndexTrasporteursModel(TmsDbContext context)
        {
            _context = context;
        }

        public IList<Transporteur> Transporteur { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Transporteurs != null)
            {
                Transporteur = await _context.Transporteurs.ToListAsync();
            }
        }
    }
}