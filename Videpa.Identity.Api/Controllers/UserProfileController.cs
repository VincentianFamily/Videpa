using System;
using System.Web.Http;
using Videpa.Identity.Api.Filters;
using Videpa.Identity.Api.ViewModels;
using Videpa.Identity.Logic.Exceptions;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Models;
using Videpa.Identity.Logic.Ports;

namespace Videpa.Identity.Api.Controllers
{
    public class UserProfileController : BaseApiController
    {
        private readonly IUserProfileCommandHandler _userProfileCmdHandler;
        private readonly IUserProfileQueries _queries;

        public UserProfileController(IUserProfileCommandHandler userProfileCommandHandler, IUserProfileQueries queries)
        {
            _userProfileCmdHandler = userProfileCommandHandler;
            _queries = queries;
        }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult CreateProfile(CreateUserProfile cmd)
        {
            var authenticatedUser = _userProfileCmdHandler.Create(cmd);

            var vm = UserProfileResponse.Build(authenticatedUser, RouteFactory);
            return Ok(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Authenticate(AuthenticateUserProfile cmd)
        {
            var authenticatedUser = _userProfileCmdHandler.Authenticate(cmd);

            var vm = UserProfileResponse.Build(authenticatedUser, RouteFactory);
            return Ok(vm);
        }

        [JwtAuthentication]
        [AuthorizeUserProfileAccess] 
        [ActionName("Profile")]
        [HttpGet]
        public IHttpActionResult GetProfile(Guid userId)
        {
            var userprofile = _queries.GetUserProfile(userId);

            if (userprofile.IsEmpty)
                throw new VidepaNotFoundException($"No user profile with id {userId} could be found");

            var vm = UserProfileResponse.Build(userprofile.Value, RouteFactory);
            return Ok(vm);
        }

        [JwtAuthentication]
        [AuthorizeUserProfileAccess]
        [ActionName("Profile")]
        [HttpGet]
        public IHttpActionResult UpdateProfile(Guid userId, UpdateUserProfile cmd)
        {
            var authenticatedUser = _userProfileCmdHandler.Update(userId, cmd);

            var vm = UserProfileResponse.Build(authenticatedUser, RouteFactory);
            return Ok(vm);
        }

        [JwtAuthentication]
        [AuthorizeUserProfileAccess]
        [HttpPost]
        public IHttpActionResult ChangeEmail(Guid userId, ChangeUserProfileEmail cmd)
        {
            var authenticatedUser = _userProfileCmdHandler.ChangeEmail(userId, cmd);

            var vm = UserProfileResponse.Build(authenticatedUser, RouteFactory);
            return Ok(vm);
        }

        [JwtAuthentication]
        [AuthorizeUserProfileAccess]
        [HttpPost]
        public IHttpActionResult ChangePassword(Guid userId, ChangeUserProfilePassword cmd)
        {
            var authenticatedUser = _userProfileCmdHandler.ChangePassword(userId, cmd);

            var vm = UserProfileResponse.Build(authenticatedUser, RouteFactory);
            return Ok(vm);
        }
    }
}
