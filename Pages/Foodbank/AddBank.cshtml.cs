#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetTopologySuite.Geometries;

#endregion

namespace Foodbank_Project.Pages.Foodbank;

public class AddBankModel : PageModel
{
    private ApplicationContext _ap;

    public AddBankModel(ApplicationContext ap)
    {
        _ap = ap;
        Foodbank = new Models.Foodbank();
    }

    [BindProperty] public Models.Foodbank Foodbank { get; set; }
    [BindProperty] public float Lat { get; set; }
    [BindProperty] public float Lng { get; set; }

    [BindProperty] public bool Consent { get; set; }

    [BindProperty] public bool Confirm { get; set; }

    public void OnGet()
    {
        Foodbank.Protected = false;
        Foodbank.Status = Status.UnConfirmed;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var geoLoc = new Point(Lat, Lng) { SRID = 4326 };
        Foodbank.Coord = geoLoc;
        Foodbank.Closed = false;
        Foodbank.Protected = true;
        Foodbank.Created = DateTime.Now;

        Foodbank = FoodbankHelpers.ApplySlug(Foodbank);
        Foodbank = FoodbankHelpers.ApplyFinalize(Foodbank);
        ModelState.ClearValidationState(nameof(Pages.Foodbank));
        if (!TryValidateModel(Foodbank, nameof(Pages.Foodbank))) return Page();

        _ap.Foodbanks?.Add(Foodbank);
        await _ap.SaveChangesAsync();
        return RedirectToPage("/Foodbank/Confirmation");
    }

    public class Coords
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}