#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Recipes;

public class SearchModel : PageModel
{
    private readonly ApplicationContext _ctx;



    public SearchModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }



    public List<Recipe>? Recipes { get; set; }
    public string? Category { get; set; }


    public async Task OnGetAsync([FromRoute(Name = "catName")] string category)
    {
        Recipes = await _ctx.Recipes.Where(r => r.Status == Status.Approved)
            .Include(r => r.Category.Where(c => c.Name == category))
            .Where(r => r.Category.Any(c => c.Name == category))
            .ToListAsync();
         Category = category;        
    }
}