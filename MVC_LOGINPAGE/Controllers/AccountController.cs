using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC_LOGINPAGE.Models;
using System.Security.Claims;

namespace MVC_LOGINPAGE.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin modelLogin)
        {

            if (modelLogin.Email == "user@example.com" && modelLogin.PassWord == "123")
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, modelLogin.Email),
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                    new Claim("OtherProperties","Example Role")              };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                 new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }



            ViewData["ValidateMessage"] = "user not found";
            return View();
        }
        public async Task<IActionResult> LogOut(string returnUrl = null)
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
