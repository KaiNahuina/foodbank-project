#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
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

    public AddBankModel(ApplicationContext ap)
    {
        _ap = ap;
    }

    public class Coords
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public double lat;

    public double lng;

    public void onGet()
    {
        foodbank.Protected = false;
        foodbank.Status = Status.UnConfirmed;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        
        var geoLoc = new Point(lng, lat) { SRID = 4326 };
        Trace.WriteLine(geoLoc.ToString());
        foodbank.Slug = foodbank.Name.ToLower();
        foodbank.Coord = geoLoc;
        foodbank.Closed = false;


        if (!ModelState.IsValid)
        {
            return Page();
        }

        _ap.Foodbanks.Add(foodbank);
        await _ap.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}