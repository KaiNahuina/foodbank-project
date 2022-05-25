namespace Foodbank_Project.Models;

public class RecipeCategory
{
    public int RecipeCategoryId { get; init; }

    public string? Name { get; init; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<Recipe>? Recipes { get; set; }
}