using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Foodbank_Project.Pages
{
    public class GiveHelpModel : PageModel
    {
        private readonly ILogger<GetHelpModel> _logger;

        public GiveHelpModel(ILogger<GetHelpModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}