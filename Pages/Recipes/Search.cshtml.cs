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



    public Models.Recipe? Recipe { get; set; }
    public IList<Recipe>? passedCategory { get; set; }
    public string Category { get; set; }


    public async Task OnGetAsync([FromRoute(Name = "catName")] string category)
    {
         passedCategory = await _ctx.RecipeCategories.AsNoTracking().Where(r => r.Name == category)
            .Include(r => r.Recipes).Select(r => new List<Recipe>(r.Recipes)).FirstOrDefaultAsync();

         Category = category;        
    }
}