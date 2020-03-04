using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace BookCluster.IdentityServer
{
    public static class Config
    {
        // Defining the API resource
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("bookclusterapi", "bookclusterapi", new [] {JwtClaimTypes.Email})
            };

        // Defining Identity resources, user assign claims to these. 
        public static IEnumerable<IdentityResource> Identity =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(), // Id
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        // Defining the Client, ClientId(login) and ClientSecrets(password) identifies the client to the identityserver.
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "BookClusterClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets =
                    {
                        new Secret("Secret".Sha256())
                    },
                    AllowedScopes = { "bookclusterapi" }
                },
                new Client
                {
                    ClientId = "BookClusterClient_Code",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets =
                    {
                        new Secret("Secret".Sha256())
                    },
                    RedirectUris = {"http://localhost:59418/signin-oidc"},
                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "bookclusterapi"
                    }
                }
            };        
    }
}
