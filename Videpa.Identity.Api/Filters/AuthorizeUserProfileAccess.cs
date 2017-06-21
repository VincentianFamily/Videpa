using System.Security.Claims;
using System.Web.Http.Controllers;
using Videpa.Identity.Api.Hypermedia;
using Videpa.Identity.Jwt;
using Videpa.Identity.Logic.Exceptions;
using Videpa.Identity.Logic.Models;

namespace Videpa.Identity.Api.Filters
{
    public class AuthorizeUserProfileAccess : AuthorizeAccessBaseAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var ex = new VidepaAuthorizationException("The current user is not authorized to access the requested user profile");

            base.OnAuthorization(actionContext);

            var user = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            
            if (user == null) throw ex;

            if (user.HasClaim(user.RoleClaimType, VidepaRoles.Vince)) return;

            var requestedUserId = FilterHelper.GetRouteParameterValue(actionContext, RouteParameters.UserId);
            if (user.HasClaim(VidepaClaims.Id, requestedUserId)) return;

            throw ex;
        }
    }
}