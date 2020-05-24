using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityDemo.IdentityService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() => 
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new[]
            {
                new ApiResource("api.weather", "Weather API")
                {
                    Scopes =
                    {
                        new Scope("api.weather.full_access", "Full access to Weather API")
                    },
                    UserClaims =
                    {
                        ClaimTypes.NameIdentifier,
                        ClaimTypes.Name,
                        ClaimTypes.Email,
                        ClaimTypes.Role,
                        "Role"
                    }
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new[]
            {
                new Client
                {
                    RequireConsent = false,
                    ClientId = "angular",
                    ClientName = "Angular SPA",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api.weather.full_access" },
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                },
                new Client
                {
                    ClientId = "webapi",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("mysecret".Sha256())
                    },
                    AlwaysSendClientClaims = true,
                    AllowedScopes = { "api.weather.full_access" }
                }
            };
    }
}
