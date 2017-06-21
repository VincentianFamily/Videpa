using System.Security.Claims;
using System.Web;

namespace Videpa.Identity.Logic.Ports
{
    public interface ICookieAuthService
    {
        void IdentitySignIn(HttpContext httpContext, ClaimsIdentity claimsIdentity);
        void IdentitySignout(HttpContext httpContext);
    }
}