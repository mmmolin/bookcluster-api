using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCluster.IdentityServer
{
    public static class Config
    {
        // Defining the API resource
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api1", "BookCluster API")
            };

        // Defining the Client, ClientId(login) and ClientSecrets(password) identifies the client to the identityserver.
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "BookClusterClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("ChangeThisSecret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                }
            };
    }
}
