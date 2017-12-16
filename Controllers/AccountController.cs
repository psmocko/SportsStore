using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.ViewModels;

namespace SportsStore.Controllers
{
  public class AccountController : Controller
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl) {
      ViewBag.returnUrl = returnUrl;
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel creds, string returnUrl) {
      if (ModelState.IsValid) {
        if (await DoLogin(creds))
        {
          return Redirect(returnUrl ?? "/");
        }
        else {
          ModelState.AddModelError("", "Invalid username or password");
        }
      }
      return View(creds);
    }

    [HttpPost]
    public async Task<IActionResult> Logout(string redirectUrl) {
      await _signInManager.SignOutAsync();
      return Redirect(redirectUrl ?? "/");
    }

    private async Task<bool> DoLogin(LoginViewModel creds) {
      var user = await _userManager.FindByNameAsync(creds.Name);
      if (user != null) {
        await _signInManager.SignOutAsync();
        var result = await _signInManager.PasswordSignInAsync(user, creds.Password, false, false);
        return result.Succeeded;
      }
      return false;
    }
  }
}
