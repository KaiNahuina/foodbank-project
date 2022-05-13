namespace Foodbank_Project.Models;

public class Recipe
{
    public int RecipeId { get; set; }

    public string Name { get; set; }

    public string? Blob { get; set; }

    public RecipeCategory? Category { get; set; }
}