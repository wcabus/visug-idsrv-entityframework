using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Sprotify.IDP
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "wesley",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Wesley"),
                        new Claim("family_name", "Cabus"),
                        new Claim("address", "Antwerp"),
                        new Claim("email", "wesley@gotsharp.be")
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new []
            {
                new Client
                {
                    ClientId = "sprotifyclient",
                    ClientName = "Sprotify",

                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "sprotifyapi"
                    },

                    RedirectUris =
                    {
                        "https://localhost:44300/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44300/signout-callback-oidc"
                    },

                    RequireConsent = false
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(), 
                new IdentityResources.Profile(), 
                new IdentityResources.Address(), 
                new IdentityResources.Phone(), 
                new IdentityResources.Email(), 
                new IdentityResource("roles", new [] { "role" })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new []
            {
                new ApiResource("sprotifyapi", "Sprotify API", new[] { "role" })
            };
        }
    }
}