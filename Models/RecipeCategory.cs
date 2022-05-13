namespace Foodbank_Project.Models;

public class RecipeCategory
{
    public int RecipeCategoryId { get; set; }

    public string? Name { get; set; }

    public ICollection<Recipe>? Recipes { get; set; }
}