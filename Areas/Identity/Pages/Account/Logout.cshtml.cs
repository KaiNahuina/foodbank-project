// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

#region

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

namespace Foodbank_Project.Areas.Identity.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;

    public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        if (returnUrl != null)
            return LocalRedirect(returnUrl);
        return RedirectToPage();
    }
}