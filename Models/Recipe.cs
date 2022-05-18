namespace Foodbank_Project.Models;

public class Recipe
{
    public int RecipeId { get; set; }

    public string Name { get; set; }
    public string Serves { get; set; }
    public string? Ingredients { get; set; }
    public string Method { get; set; }
    public string? Notes { get; set; }
    public byte[]? Image { get; set; }
    public Status? Status { get; set; }

    public ICollection<RecipeCategory>? Category { get; set; }
}