using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sprotify.Web.Services;

namespace Sprotify.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public AccountController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [Route("/signin")]
        public async Task<IActionResult> Login(string redirectUrl = null)
        {
            // When returning with an authenticated user, make sure our Sprotify API knows this user
            if (User.Identity.IsAuthenticated)
            {
                await _userService.EnsureUserExists(User.GetSubject(), User.GetGivenName());
            }

            if (string.IsNullOrWhiteSpace(redirectUrl) || !Url.IsLocalUrl(redirectUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(redirectUrl);
        }

        [Route("/signout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        [Route("/signup")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var redirectUrl = Url.Action("Login", "Account", null, "https");
            return Redirect($"{_configuration.GetValue<string>("Authority")}signup?returnUrl={redirectUrl}");
        }
    }
}