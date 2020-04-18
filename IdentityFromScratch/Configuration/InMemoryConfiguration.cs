using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityFromScratch
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources() =>
              new List<ApiResource>() { new ApiResource("api", "API") };

        public static IEnumerable<Client> GetClients()
        {
            return new[] {
             new Client{
                ClientId="api",
                ClientSecrets= new []{new Secret("o90IbCACXKUkunXoa18cODcLKnQTbjOo5ihEw9j58+8=")},
                AllowedGrantTypes= new [] {GrantType.ResourceOwnerPassword},
                AllowedScopes= new [] {"api"}
             }

            };
        }

        public static IEnumerable<TestUser> TestUsers()
        {
            return new[]{ new TestUser{
                      SubjectId="1",
                      Username="email@gmail.com",
                      Password="password"
                  }
              };
        }
    }
}