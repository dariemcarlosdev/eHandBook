using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorClient.Pages
{
    public class LoginModel : PageModel
    {
        //Get rid of the Onget() method.
        //public void OnGet()
        //{
        //}

        public async Task<IActionResult> OnGetAsync( string redirectUri) 
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = Url.Content("~/");

            }
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect(redirectUri);
            }

            //await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme);

            return Challenge(new AuthenticationProperties { RedirectUri = redirectUri },OpenIdConnectDefaults.AuthenticationScheme);
        }

    }
}
