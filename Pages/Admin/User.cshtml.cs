#region

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "UsersAdmin,SiteAdmin")]
public class UserModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public string? Action;
    
    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;
    public string? OrderBy;
    public string? OrderDirection;
    public new int Page;
    public string? Search;
    public int TotalItems;
    
    public IList<string>? Roles;

    public UserModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [Required]
    [BindProperty] public string? Id { get; set; }
    
    [Required]
    [BindProperty] public bool Locked { get; set; }
    
    [Required]
    [EmailAddress]
    [BindProperty] public string? Email { get; set; }
    
    [DataType(DataType.Password)]
    [BindProperty] public string? Password { get; set; }
    
    [Required]
    [BindProperty] public int FoodbankClaim { get; set; }

    public async Task OnGetAsync([FromQuery(Name = "Action")] string? action, [FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
        Action = action ?? "Update";
        if (Action != "Create")
        {
            var id = RouteData.Values["id"] as string ?? "";
            OrderBy = string.IsNullOrEmpty(orderBy) ? "Name" : orderBy;
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

            var u = await _userManager.FindByIdAsync(id);

            var roleQue = (await _userManager.GetRolesAsync(u)).AsQueryable();
            roleQue = roleQue.Where(n =>
                string.IsNullOrEmpty(Search) || n.Contains(Search));
            


            switch (OrderDirection)
            {
                case "Asc":
                {
                    roleQue = OrderBy switch
                    {
                        "Name" => roleQue.OrderBy(n => n),
                        _ => roleQue
                    };
                    break;
                }
                case "Desc":
                {
                    roleQue = OrderBy switch
                    {
                        "Name" => roleQue.OrderByDescending(n => n),
                        _ => roleQue
                    };
                    break;
                }
            }
            
            HasPrevPage = Page > 1;

            TotalItems = roleQue.Count();
            MaxPages = (int)Math.Ceiling(TotalItems / 25d);

            HasNextPage = Page < MaxPages;

            Roles = roleQue.Skip((Page - 1) * 25).Take(25).ToList();

            Email = u.Email;
            Id = u.Id;
            Locked = await _userManager.IsLockedOutAsync(u);
            foreach (var claim in await _userManager.GetClaimsAsync(u))
            {
                if (claim.Type == "FoodbankClaim")
                {
                    FoodbankClaim = int.Parse(claim.Value);
                }
            }
            
        }
    }


    public async Task<IActionResult> OnPostAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Delete":
            {
                var u = await _userManager.FindByIdAsync(Id);
                var result = await _userManager.DeleteAsync(u);
                if (!result.Succeeded)
                {
                    foreach (var identityError in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, identityError.Code + " :: " + identityError.Description);
                    }

                    return Page();
                }

                break;
            }
            case "Create":
            {
                if (!ModelState.IsValid) return Page();

                var u = new IdentityUser
                {
                    UserName = Email,
                    Email = Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };
                if (Locked)
                {
                    await _userManager.SetLockoutEndDateAsync(u, DateTimeOffset.MaxValue);
                }
                else
                {
                    await _userManager.SetLockoutEndDateAsync(u, DateTimeOffset.Now);
                }
                
                var result = await _userManager.CreateAsync(u, Password);
                if (!result.Succeeded)
                {
                    foreach (var identityError in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, identityError.Code + " :: " + identityError.Description);
                    }

                    return Page();
                }
                await _userManager.AddClaimAsync(u, new Claim("FoodbankClaim", FoodbankClaim.ToString()));
                break;
            }
            case "Update":
            {
                if (!ModelState.IsValid) return Page();
                
                var u = await _userManager.FindByIdAsync(Id);
                u.Email = Email;
                u.UserName = Email;
                if (Locked)
                {
                    await _userManager.SetLockoutEndDateAsync(u, DateTimeOffset.MaxValue);
                }
                else
                {
                    await _userManager.SetLockoutEndDateAsync(u, DateTimeOffset.Now);
                }

                if (Password is not null)
                {
                    await _userManager.RemovePasswordAsync(u);

                    await _userManager.AddPasswordAsync(u, Password);
                }

               
                
                await _userManager.UpdateAsync(u);
                await _userManager.AddClaimAsync(u, new Claim("FoodbankClaim", FoodbankClaim.ToString()));
                break;
            }

        }
        
        return RedirectToPage("./Users");
    }
}