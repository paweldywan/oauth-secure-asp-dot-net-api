using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;
using SocialNetwork.OAuth.Configuration;

[assembly: OwinStartup(typeof(SocialNetwork.OAuth.Startup))]

namespace SocialNetwork.OAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var factory = new IdentityServerServiceFactory()
                           .UseInMemoryUsers(Users.GetUsers())
                           .UseInMemoryClients(Clients.GetClients())
                           .UseInMemoryScopes(Scopes.GetScopes());
            
            var options = new IdentityServerOptions
            {
                SigningCertificate = new X509Certificate2(Convert.FromBase64String(ConfigurationManager.AppSettings["SigningCertificate"]), ConfigurationManager.AppSettings["SigningCertificatePassword"]),
                Factory = factory,
                RequireSsl = false,
                AuthenticationOptions = new AuthenticationOptions { EnablePostSignOutAutoRedirect = true }
            };

            app.UseIdentityServer(options);
        }
    }
}
