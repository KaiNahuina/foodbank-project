using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Pages.Admin
{
    public class FoodbankModel : PageModel
    {
        private readonly ApplicationContext _ctx;

        public Foodbank Foodbank { get; set; }
        public FoodbankModel(ApplicationContext ctx)
        {
            _ctx = ctx;
        }
        public async Task OnGetAsync()
        {
            int id = int.Parse(RouteData.Values["id"] as string);
            IQueryable<Foodbank> foodbanks = from f in _ctx.Foodbanks where f.FoodbankId == id select f;

            Foodbank = await foodbanks.AsNoTracking().Include(f => f.Locations).Include(f => f.Needs).FirstAsync();
        }
    }
}
