#region

using Foodbank_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Foodbank;

public class EditBankModel : PageModel
{
    private readonly ApplicationContext _ap;

    public EditBankModel(ApplicationContext ap)
    {
        _ap = ap;
    }

    public Models.Foodbank? FoodEdit { get; set; }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        if (!ModelState.IsValid) return Page();

        if (FoodEdit == null) return Page();

        _ap.Attach(FoodEdit).State = EntityState.Modified;
        try
        {
            await _ap.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new Exception($"Item {FoodEdit.FoodbankId} not found!", e);
        }

        return RedirectToPage("/Index");
    }
}