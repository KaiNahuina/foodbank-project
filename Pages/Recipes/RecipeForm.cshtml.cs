#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

namespace Foodbank_Project.Pages;

public class RecipeFormModel : PageModel
{
    [BindProperty]
    public Models.Recipe Recipe { get; set; } = new();
    [BindProperty]
    public  IFormFile Image { get; set; }

    public ICollection<RecipeCategory> Categories { get; set; }


    private ApplicationContext _ctx;
   
    public RecipeFormModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public void OnGet()
    {
        Categories = _ctx.RecipeCategories.ToArray();
    }
    

    public async Task<IActionResult> OnPostAsync()
    {
        
        
        MemoryStream ms = new MemoryStream();
            Image.CopyTo(ms);
            Recipe.Image = ms.ToArray();
            ms.Close();
            ms.Dispose();



        ModelState.ClearValidationState(nameof(Recipe));
        if (!TryValidateModel(Recipe, nameof(Recipe)))
        {
            return Page();
        }
        _ctx.Recipes.Add(Recipe);
        await _ctx.SaveChangesAsync();
        return RedirectToPage("/Recipes/RecipeForm");
    }

}