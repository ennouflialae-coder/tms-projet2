using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TMS_Project.Pages
{
    public class AccueilModel : PageModel
    {
        private readonly ILogger<AccueilModel> _logger;

        public AccueilModel(ILogger<AccueilModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}