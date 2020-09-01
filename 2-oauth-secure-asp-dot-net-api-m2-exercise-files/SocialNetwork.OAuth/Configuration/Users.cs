using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;

namespace SocialNetwork.OAuth.Configuration
{
    public class Users
    {
        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "mail@filipekberg.se",
                    Username = "mail@filipekberg.se",
                    Password = "password",
                    Claims = new []
                    {
                        new Claim(Constants.ClaimTypes.Name, "Filip Ekberg")
                    }
                }
            };
        }
    }
}