#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

namespace Foodbank_Project.Pages.Recipes;

public class RecipeFormModel : PageModel
{
    private ApplicationContext _ctx;

    public RecipeFormModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    [BindProperty] public Recipe Recipe { get; set; } = new();
    [BindProperty] public IFormFile? Image { get; set; }
    [BindProperty] public Dictionary<int, Pair<RecipeCategory, bool>> SelectedCategories { get; set; } = new();

    public void OnGet()
    {
        foreach (var category in _ctx.RecipeCategories!.ToArray())
            SelectedCategories.Add(category.RecipeCategoryId,
                new Pair<RecipeCategory, bool> { Item1 = category, Item2 = false });
    }


    public async Task<IActionResult> OnPostAsync()
    {
        foreach (var category in _ctx.RecipeCategories!.ToArray())
            SelectedCategories[category.RecipeCategoryId].Item1 = category;

        Recipe.Categories = new List<RecipeCategory>();


        if (Image != null)
        {
            var ms = new MemoryStream();
            await Image?.CopyToAsync(ms)!;
            Recipe.Image = ms.ToArray();
            ms.Close();
            await ms.DisposeAsync();
        }

        foreach (var category in SelectedCategories.Where(category => category.Value.Item2))
            Recipe.Categories.Add(category.Value.Item1!);

        Recipe.Status = Status.UnConfirmed;


        ModelState.ClearValidationState(nameof(Recipe));
        if (!TryValidateModel(Recipe, nameof(Recipe))) return Page();
        _ctx.Recipes?.Update(Recipe);
        await _ctx.SaveChangesAsync();
        return RedirectToPage("/Foodbank/Confirmation");
    }

    public class Pair<T1, T2>
    {
        public T1? Item1 { get; set; }
        public T2? Item2 { get; set; }
    }
}