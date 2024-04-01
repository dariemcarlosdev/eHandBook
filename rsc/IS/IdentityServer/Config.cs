// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.Json;

namespace IdentityServer
{
    public static class Config
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
        {
          new TestUser
          {
            SubjectId = "818727",
            Username = "alice",
            Password = "alice",
            Claims =
            {
              new Claim(JwtClaimTypes.Name, "Alice Smith"),
              new Claim(JwtClaimTypes.GivenName, "Alice"),
              new Claim(JwtClaimTypes.FamilyName, "Smith"),
              new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "admin"),
              new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
              new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                IdentityServerConstants.ClaimValueTypes.Json)
            }
          },
          new TestUser
          {
            SubjectId = "88421113",
            Username = "bob",
            Password = "bob",
            Claims =
            {
              new Claim(JwtClaimTypes.Name, "Bob Smith"),
              new Claim(JwtClaimTypes.GivenName, "Bob"),
              new Claim(JwtClaimTypes.FamilyName, "Smith"),
              new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "user"),
              new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
              new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                IdentityServerConstants.ClaimValueTypes.Json)
            }
          }
        };
            }
        }


        //Identity Resources. This are thing associated with a given identity
        //Adding support for the Identity resources openid (subject id) and profile (first name, last name etc..). These represent some standard OIDC Scopes that we want IS to support.
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                 new IdentityResources.OpenId(), //OpenId Scope is always required if we are using Open Id.
                 new IdentityResources.Profile(),
                 new IdentityResource
                 {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                 }
            };

        //Scopes.
        /*An API is a resource in your system that you want to protect.the scope of access that the client requests.*/
        public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("weatherapi.read", " Read API"),
            new ApiScope("weatherapi.write", "Write API")
        };

        //Resources: It's something we are trying to protects Here we have apiResources named api1, have to Scope assciated with it { "api1.read", "api1.write"}
        //it means there is something that can read api1, and something that can add/update/delete items using api1.
        public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
           new ApiResource("weatherapi")
           {
            Scopes = new List<string>{
                "weatherapi.read",
                "weatherapi.write"
            },
            ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
            UserClaims = new List<string>{"role" }
           }
        };


        /*
         You can think of the ClientId and the ClientSecret as the login and password for your application itself. 
        It identifies your client application (apps will be talking to IS) to the identity server so that it knows which client application is trying to connect to it.
        Client will be talking to  the identity server to be asked to be allowed to use our applications.
        Clients represent applications that can request tokens from your identityserver.
         */
        public static IEnumerable<Client> Clients =>
        new List<Client>
        {

        new Client
            {
            //machine to machine(m2m) client credentials flow client app. (from quickstart 1).The client names should be meaningful to your applications.
            // Represents a machine identity that can access your APIs without the need for a human to authenticate the access.
            ClientId = "m2m.client",
            //  It's store into de IS.Secret for authentication plus ClientId and ask to IS for a Token.
            ClientSecrets = { new Secret("secret".Sha256()) },

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            // scopes that client has access to
            AllowedScopes = { "weatherapi.read","weatherapi.write" }
            },
        
        // interactive ASP.NET Core MVC client using code flow + pkce.
         //since the flows in OIDC are always interactive, we need to add some redirect URLs to our configuration.
        new Client
        {
            ClientId = "interactive.client",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,

            // where to redirect to after login
            RedirectUris = { "https://localhost:5002/signin-oidc" },
            FrontChannelLogoutUri ="https://localhost:5002/signout-oidc",
            // where to redirect to after logout
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
            AllowOfflineAccess = true,
            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "weatherapi.read"
            },
            RequirePkce = true,
            RequireConsent = true,
            AllowPlainTextPkce = false
        }
        };
    }
}