using IdentityServer4.Models;
using IdentityServer4.Test;
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
                new ApiResource("bookclusterapi", "bookclusterapi")
            };

        // Defining the Client, ClientId(login) and ClientSecrets(password) identifies the client to the identityserver.
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "BookClusterClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets =
                    {
                        new Secret("Secret".Sha256())
                    },
                    AllowedScopes = { "bookclusterapi" }
                }
            };

        public static List<TestUser> Users =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "testuser@user.se",
                    Password = "password"
                }
            };
    }
}
