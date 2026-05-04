using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Pages.Transporteurs
{
    public class CreateModel : PageModel
    {
        private readonly TmsDbContext _context;

        public CreateModel(TmsDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Transporteur Transporteur { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Transporteurs.Add(Transporteur);
            await _context.SaveChangesAsync();
            return RedirectToPage("./IndexTrasporteurs");
        }
    }
}