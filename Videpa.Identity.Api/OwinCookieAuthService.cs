using System;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Videpa.Identity.Logic.Ports;

namespace Videpa.Identity.Api
{
    public class OwinCookieAuthService : ICookieAuthService
    {
        public void IdentitySignIn(HttpContext httpContext, ClaimsIdentity claimsIdentity)
        {
            var authManager = httpContext.GetOwinContext().Authentication;

            authManager.SignIn(new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, claimsIdentity);
        }

        public void IdentitySignout(HttpContext httpContext)
        {
            var authManager = httpContext.GetOwinContext().Authentication;

            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}