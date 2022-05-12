using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Pages.Admin
{
    public class FoodbanksModel : PageModel
    {
        private readonly ApplicationContext _ctx;
        
        public IList<Foodbank> Foodbanks;

        public FoodbanksModel(ApplicationContext ctx)
        {
            _ctx = ctx;
        }
        
        public async Task OnGetAsync()
        {
            IQueryable<Foodbank> foodbankQue = from f in _ctx.Foodbanks
                select f;

            // do sort and filter shizzle here

            Foodbanks = await foodbankQue.AsNoTracking().ToListAsync();

        }
    }
}
