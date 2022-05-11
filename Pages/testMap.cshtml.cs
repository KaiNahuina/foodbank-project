using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Foodbank_Project.Pages
{
    public class testMapModel : PageModel
    {
        public string? Location { get; set; }
        public void OnGet()
        {
            Location = RouteData.Values?["Location"]?.ToString() ?? null;
        }
    }
}
