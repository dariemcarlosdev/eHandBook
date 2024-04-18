using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [Route("identity")]

    // This authorize attribute challenges all clients attempting to access all controller methods.
    // Clients must posses the client scope claim "identityApi" (api resource in IdentityServer)
    // It is not actually required in this specific case, because there is only one method and it has its own Authorize attribute.
    // However, it is a common practice to have this controller level attribute to ensure that Identity Server is protecting the entire controller, including methods that may be added in the future.
    [Authorize]
    public class IdentityController : ControllerBase
            {
        [HttpGet]
        // Use samed shared authorization policy to protect the api GET method that is used to protect the application feature
        // This checks for the user claim type appRole_Claim with value "identity".
        [Authorize(Policy = Policies.CanViewIdentity)]
        public IActionResult Get()
                {
                    return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
                 }
}
         }

