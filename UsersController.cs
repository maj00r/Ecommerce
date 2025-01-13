using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly SignInManager<ApplicationUser> signInManager = signInManager;

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([Required] LoginPayload loginPayload)
    {
        ApplicationUser appUser = await userManager.FindByEmailAsync(loginPayload.Email);
        Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, loginPayload.Password, false, false);

        if (result.Succeeded)
        {
            return Redirect("/");
        }
        return BadRequest();

    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Redirect("/");
    }

    [Authorize]
    [HttpPost("verify")]
    public async Task<IActionResult> Verify()
    {
        if (User.Identity.IsAuthenticated)
        {
            return Ok();
        }
        return Unauthorized();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(User user)
    {
        ApplicationUser appUser = new ApplicationUser
        {
            UserName = user.Name,
            Email = user.Email
        };

        IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
        return Ok(result);
    }
}