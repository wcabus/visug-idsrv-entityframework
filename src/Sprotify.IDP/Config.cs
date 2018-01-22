using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Sprotify.IDP
{
    public static class Config
    {
        public static List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "sprotifyclient",
                    ClientName = "Sprotify",

                    AllowedGrantTypes = GrantTypes.Hybrid,
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
                    
                    AllowOfflineAccess = true,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    RequireConsent = false
                }
            };
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResource("roles", new [] { "role" })
            };
        }

        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("sprotifyapi", "Sprotify API", new List<string>{ "role" })
            };
        }
    }
}