#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

namespace Foodbank_Project.Pages.Foodbank;

public class AddBankModel : PageModel
{

    private ApplicationContext _ap;

    public Models.Foodbank foodbank { get; set; }

    private Need need { get; set; }

    public string needs;

    public string add1;

    public string? add2;

    public string city;

    public string? region;

    private string fullAddress;

    public string opening;

    public string closing;

    private Tuple<string> openingTimes;


    public AddBankModel(ApplicationContext ap)
    {
        _ap = ap;
    }

    public async Task<IActionResult> OnPost()
    {
        if(!ModelState.IsValid)
        {
            return Page();
        }
        fullAddress = $"{add1} {add2} {city} {region}";
        foodbank.Address = fullAddress;
        foodbank.Closed = false;
        foodbank.Status = Status.UnConfirmed;
        _ap.Foodbanks.Add(foodbank);
        await _ap.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
    public void OnGet()
    {
    }
}