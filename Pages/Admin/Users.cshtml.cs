#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "UsersAdmin,SiteAdmin")]
public class UsersModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;
    public string? OrderBy;
    public string? OrderDirection;
    public new int Page;
    public string? Search;
    public int TotalItems;

    public IList<IdentityUser>? Users;

    public UsersModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task OnGetAsync([FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
        OrderBy = string.IsNullOrEmpty(orderBy) ? "Count" : orderBy;
        OrderDirection = string.IsNullOrEmpty(orderDirection) ? "Desc" : orderDirection;
        if (!int.TryParse(page, out Page)) Page = 1;
        if (string.IsNullOrEmpty(search))
        {
            if (string.IsNullOrEmpty(Search)) Search = "";
        }
        else
        {
            Search = search;
        }

        var userQue = (from f in _userManager.Users
                select f).AsNoTracking()
            .OrderByDescending(n => n.Email).Where(n =>
                string.IsNullOrEmpty(Search) || n.Email!.Contains(Search) || n.Id == Search);


        switch (OrderDirection)
        {
            case "Asc":
            {
                userQue = OrderBy switch
                {
                    "Name" => userQue.OrderBy(n => n.Email),
                    _ => userQue
                };

                break;
            }
            case "Desc":
            {
                userQue = OrderBy switch
                {
                    "Name" => userQue.OrderByDescending(n => n.Email),
                    _ => userQue
                };

                break;
            }
        }

        HasPrevPage = Page > 1;

        TotalItems = await userQue.CountAsync();
        MaxPages = (int)Math.Ceiling(TotalItems / 25d);

        HasNextPage = Page < MaxPages;

        Users = await userQue.Skip((Page - 1) * 25).Take(25).ToListAsync();
    }
}