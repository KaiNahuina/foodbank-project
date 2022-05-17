using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Pages
{
    public class EditBankModel : PageModel
    {
        private readonly ApplicationContext _ap;
        public EditBankModel(ApplicationContext ap)
        {
            _ap = ap;
        }

        public Foodbank foodEdit { get; set; }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            _ap.Attach(foodEdit).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                await _ap.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Item {foodEdit.FoodbankId} not found!", e);
            }
            return RedirectToPage("/Index");

        }
        public void OnGet()
        {
        }
    }
}
