#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

#endregion

namespace Foodbank_Project.Pages.Foodbank;

public class AddBankModel : PageModel
{

    private ApplicationContext _ap;

    [BindProperty]
    public Models.Foodbank foodbank { get; set; }
    public float lat { get; set; }
    public float lng { get; set; }

    public AddBankModel(ApplicationContext ap)
    {
        _ap = ap;
    }

    public void onGet()
    {
        foodbank.Protected = false;
        foodbank.Status = Status.UnConfirmed;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var geoLoc = new Point(lat, lng) { SRID = 4326 };
        Trace.WriteLine(geoLoc.ToString());
        foodbank.Coord = geoLoc;
        foodbank.Closed = false;
        foodbank.Protected = true;
        foodbank.Created = DateTime.Now;

        foodbank = FoodbankHelpers.ApplySlug(foodbank);
        foodbank = FoodbankHelpers.ApplyFinalize(foodbank);
        ModelState.ClearValidationState(nameof(Foodbank));
        if (!TryValidateModel(foodbank, nameof(Foodbank)))
        {
            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            return Page();
        }
        _ap.Foodbanks.Add(foodbank);
        await _ap.SaveChangesAsync();
        return RedirectToPage("/Index");
    }

    public class Coords
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}