using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Pages.Transporteurs
{
    public class EditModel : PageModel
    {
        private readonly TmsDbContext _context;

        public EditModel(TmsDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Transporteur Transporteur { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transporteur =  await _context.Transporteurs.FirstOrDefaultAsync(m => m.TransporteurId == id);
            if (transporteur == null)
            {
                return NotFound();
            }
            Transporteur = transporteur;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Transporteur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransporteurExists(Transporteur.TransporteurId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./IndexTrasporteurs");
        }

        private bool TransporteurExists(int id)
        {
            return _context.Transporteurs.Any(e => e.TransporteurId == id);
        }
    }
}
