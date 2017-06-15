using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Videpa.Identity.Api.Controllers;
using Videpa.Identity.Logic.Exceptions;

namespace Videpa.Identity.Api.Filters
{
    public abstract class AuthorizeAccessBaseAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var ctrl = actionContext.ControllerContext.Controller as BaseApiController;
            IsApiBaseController(ctrl);
            IsAuthenticated(ctrl);
        }

        protected static void IsApiBaseController(BaseApiController ctrl)
        {
            if (ctrl == null)
                throw new Exception("Operation is used on a non-ApiBaseController");
        }

        protected static void IsAuthenticated(BaseApiController ctrl)
        {
            if (!ctrl.User.Identity.IsAuthenticated)
                throw new VidepaAuthorizationException("The current user is not authenticated");
        }
    }
}