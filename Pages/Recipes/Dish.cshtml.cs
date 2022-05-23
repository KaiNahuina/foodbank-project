#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Recipes;

public class DishModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public Recipe Recipe { get; set; }

    public DishModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task OnGetAsync([FromRoute]int id)
    {
        Recipe = await _ctx.Recipes.Where(r => r.RecipeId == id).FirstAsync();
    }
}