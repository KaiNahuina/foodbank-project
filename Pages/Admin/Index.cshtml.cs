using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Pages.Admin
{

    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _ctx;

        public IList<Foodbank> Foodbanks;

        public IndexModel(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public async Task OnGetAsync()
        {
            IQueryable<Foodbank> foodbankQue = from f in _ctx.Foodbanks
                                               where f.Status == Status.UnConfirmed
                                             select f;

            // do sort and filter shizzle here

            Foodbanks = await foodbankQue.AsNoTracking().ToListAsync();

        }
    }
}
