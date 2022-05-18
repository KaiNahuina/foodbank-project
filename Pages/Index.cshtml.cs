#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

namespace Foodbank_Project.Pages;

public class IndexModel : PageModel
{
    [BindProperty] public string Location { get; set; }

    public void OnGet()
    {
    }
}