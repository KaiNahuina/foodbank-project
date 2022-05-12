using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Pages.Admin
{
    public class ContentModel : PageModel
    {
        private readonly ApplicationContext _ctx;
        
        public IList<Content> Contents;
        
        public ContentModel(ApplicationContext ctx)
        {
            _ctx = ctx;
        }
        
        public async Task OnGetAsync()
        {
            IQueryable<Content> foodbankQue = from f in _ctx.Contents
                select f;

            // do sort and filter shizzle here

            Contents = await foodbankQue.AsNoTracking()
                .Select(c => new Content{ContentId = c.ContentId, Name = c.Name,Blob = TrimBlob(c.Blob)})
                .ToListAsync();

        }

        private static string TrimBlob(string? blob)
        {
            if (blob is null) return "";
            if (blob.Length > 32)
            {
                return blob.Substring(0, 32) + " ...";
            }
            else
            {
                return blob;
            }
        }
    }
}
