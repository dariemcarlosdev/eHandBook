using Microsoft.AspNetCore.Authorization;

namespace Api
{
    /// <summary>
    /// A claims-based authorization policy.
    /// </summary>
    public static class Policies
    {
        public const string CanViewIdentity = "CanViewIdentity";

        public static AuthorizationPolicy CanViewIdentityPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("appUser_claim", "identity")
                .Build();
        }
    }
}