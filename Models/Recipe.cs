#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Foodbank_Project.Models;

public class Recipe
{
    public int RecipeId { get; init; }

    [Required(ErrorMessage = "Recipe must have a name")]
    public string? Name { get; set; }

    public string? Serves { get; init; }

    [Required(ErrorMessage = "Recipe requires a list of ingredients")]
    public string? Ingredients { get; set; }

    [Required(ErrorMessage = "Recipe must have preparation instructions")]
    public string? Method { get; set; }

    public string? Notes { get; set; }
    public byte[]? Image { get; set; }
    public Status? Status { get; set; }


    public ICollection<RecipeCategory>? Categories { get; set; }
}