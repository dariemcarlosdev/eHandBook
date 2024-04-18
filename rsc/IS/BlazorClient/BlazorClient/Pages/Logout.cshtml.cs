using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlazorClient.Pages
{
    public class LogoutModel : PageModel
    {
        //public void OnGet()
        //{
        //}

        private readonly IConfiguration _config;

        public LogoutModel(IConfiguration configuration)
        {
                _config = configuration;
        }

        public async Task/*<IActionResult> */ OnGetAsync()
        {
            //return SignOut(
            //    new AuthenticationProperties
            //    {
            //        RedirectUri = _config["applicationUrl"]
            //    },
            //    OpenIdConnectDefaults.AuthenticationScheme,
            //    CookieAuthenticationDefaults.AuthenticationScheme);
            // Sign out of Cookies and OIDC schemes
            await HttpContext.SignOutAsync( CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = _config["applicationUrl"] });
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = _config["applicationUrl"] });
        }
    }
}
