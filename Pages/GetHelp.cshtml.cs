using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Foodbank_Project.Pages
{
    public class GetHelpModel : PageModel
    {
        private readonly ILogger<GetHelpModel> _logger;

        public GetHelpModel(ILogger<GetHelpModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}