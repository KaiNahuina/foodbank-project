#region

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "UsersAdmin,SiteAdmin")]
public class UserModel : PageModel
{
    private readonly ILogger<UserModel> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public string? Action;

    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;
    public string? OrderBy;
    public string? OrderDirection;
    public new int Page;

    public IList<string>? Roles;
    public string? Search;
    public int TotalItems;

    public UserModel(UserManager<IdentityUser> userManager, ILogger<UserModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [Required] [BindProperty] public string? Id { get; set; }

    [Required] [BindProperty] public bool Locked { get; set; }

    [Required]
    [EmailAddress]
    [BindProperty]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    [BindProperty]
    public string? Password { get; set; }

    [Required] [BindProperty] public int FoodbankClaim { get; set; }

    public async Task OnGetAsync([FromQuery(Name = "Action")] string? action,
        [FromQuery(Name = "OrderBy")] string? orderBy,
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

            roleQue = OrderDirection switch
            {
                "Asc" => OrderBy switch
                {
                    "Name" => roleQue.OrderBy(n => n),
                    _ => roleQue
                },
                "Desc" => OrderBy switch
                {
                    "Name" => roleQue.OrderByDescending(n => n),
                    _ => roleQue
                },
                _ => roleQue.Where(n => string.IsNullOrEmpty(Search) || n.Contains(Search))
            };

            HasPrevPage = Page > 1;

            TotalItems = roleQue.Count();
            MaxPages = (int)Math.Ceiling(TotalItems / 25d);

            HasNextPage = Page < MaxPages;

            Roles = roleQue.Skip((Page - 1) * 25).Take(25).ToList();

            Email = u.Email;
            Id = u.Id;
            Locked = await _userManager.IsLockedOutAsync(u);
            foreach (var claim in await _userManager.GetClaimsAsync(u))
                if (claim.Type == "FoodbankClaim")
                    FoodbankClaim = int.Parse(claim.Value);
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
                        ModelState.AddModelError(string.Empty, identityError.Code + " :: " + identityError.Description);

                    return Page();
                }

                _logger.Log(LogLevel.Warning, "User {UserName} removed user {TargetUser}",
                    User.Identity?.Name, u.UserName);

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
                    PhoneNumberConfirmed = true
                };
                if (Locked)
                    await _userManager.SetLockoutEndDateAsync(u, DateTimeOffset.MaxValue);
                else
                    await _userManager.SetLockoutEndDateAsync(u, DateTimeOffset.Now);

                var result = await _userManager.CreateAsync(u, Password);
                if (!result.Succeeded)
                {
                    foreach (var identityError in result.Errors)
                        ModelState.AddModelError(string.Empty, identityError.Code + " :: " + identityError.Description);

                    return Page();
                }

                await _userManager.AddClaimAsync(u, new Claim("FoodbankClaim", FoodbankClaim.ToString()));

                _logger.Log(LogLevel.Information, "User {UserName} created user {TargetUser}",
                    User.Identity?.Name, u.UserName);

                break;
            }
            case "Update":
            {
                if (!ModelState.IsValid) return Page();

                var u = await _userManager.FindByIdAsync(Id);
                u.Email = Email;
                u.UserName = Email;
                if (Locked)
                    await _userManager.SetLockoutEndDateAsync(u, DateTimeOffset.MaxValue);
                else
                    await _userManager.SetLockoutEndDateAsync(u, DateTimeOffset.Now);

                if (Password is not null)
                {
                    await _userManager.RemovePasswordAsync(u);

                    await _userManager.AddPasswordAsync(u, Password);
                }


                foreach (var claim in await _userManager.GetClaimsAsync(u))
                    if (claim.Type == "FoodbankClaim")
                        await _userManager.RemoveClaimAsync(u, claim);

                await _userManager.AddClaimAsync(u, new Claim("FoodbankClaim", FoodbankClaim.ToString()));
                await _userManager.UpdateAsync(u);


                _logger.Log(LogLevel.Information, "User {UserName} updated user {TargetUser}",
                    User.Identity?.Name, u.UserName);

                break;
            }
        }

        return RedirectToPage("./Users");
    }
}