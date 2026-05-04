using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Pages.Transporteurs
{
    public class DetailsModel : PageModel
    {
        private readonly TmsDbContext _context;

        public DetailsModel(TmsDbContext context)
        {
            _context = context;
        }

        public Transporteur Transporteur { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transporteur = await _context.Transporteurs.FirstOrDefaultAsync(m => m.TransporteurId == id);

            if (transporteur is not null)
            {
                Transporteur = transporteur;

                return Page();
            }

            return NotFound();
        }
    }
}
