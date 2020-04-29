using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityFromScratch
{
    public class InMemoryConfiguration
    {

        public static IEnumerable<IdentityResource> IdentityResources() =>
            new List<IdentityResource>() { new IdentityResources.OpenId(), new IdentityResources.Profile() };


        public static IEnumerable<ApiResource> ApiResources() =>
              new List<ApiResource>() { new ApiResource("api", "API") };

        public static IEnumerable<Client> GetClients()
        {
            return new[] {
             new Client{
                ClientId="api",
                ClientSecrets= new []{new Secret("o90IbCACXKUkunXoa18cODcLKnQTbjOo5ihEw9j58+8=")},
                AllowedGrantTypes= new [] {GrantType.ResourceOwnerPassword},
                AllowedScopes= new [] {"profile","openid"}
             }

            };
        }

        public static IEnumerable<TestUser> TestUsers()
        {
            return new[]{ new TestUser{
                      SubjectId="1",
                      Username="email@gmail.com",
                      ProviderName="Mayko Estevez",
                      Password="password",
                    Claims =  new List<Claim>()
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }

                  }
              };
        }
    }
}