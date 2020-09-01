using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Thinktecture.IdentityModel.Clients;

namespace SocialNetwork.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var client = new OAuth2Client(new Uri("http://localhost:22710/connect/token"),
                "socialnetwork", "secret");

            var requestResponse = client.RequestAccessTokenUserName(username, password,
                "openid profile offline_access");

            var claims = new[]
            {
                new Claim("access_token", requestResponse.AccessToken),
                new Claim("refresh_token", requestResponse.RefreshToken)
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, 
                DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(claimsIdentity);

            return Redirect("/private");
        }

        public ActionResult RefreshAccessToken()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            
            var client = new OAuth2Client(new Uri("http://localhost:22710/connect/token"),
                "socialnetwork", "secret");

            var requestResponse = client.RequestAccessTokenRefreshToken(
                claimsPrincipal.FindFirst("refresh_token").Value);

            var manager = HttpContext.GetOwinContext().Authentication;

            var refreshedIdentity = new ClaimsIdentity(User.Identity);

            refreshedIdentity.RemoveClaim(refreshedIdentity.FindFirst("access_token"));
            refreshedIdentity.RemoveClaim(refreshedIdentity.FindFirst("refresh_token"));

            refreshedIdentity.AddClaim(new Claim("access_token",
                requestResponse.AccessToken));

            refreshedIdentity.AddClaim(new Claim("refresh_token",
                requestResponse.RefreshToken));

            manager.AuthenticationResponseGrant =
                new AuthenticationResponseGrant(new ClaimsPrincipal(refreshedIdentity),
                new AuthenticationProperties { IsPersistent = true });

            return Redirect("/private");
        }

        [Authorize]
        public async Task<ActionResult> Private()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Authorization =
            //        new AuthenticationHeaderValue("Bearer", 
            //        claimsPrincipal.FindFirst("access_token").Value);

            //    var profile = await client.GetAsync("http://localhost:3467/api/Profiles");

            //    var profileContent = await profile.Content.ReadAsStringAsync();
            //}

            return View(claimsPrincipal.Claims);
        }

        [Authorize]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();

            return Redirect("/");
        }
    }
}