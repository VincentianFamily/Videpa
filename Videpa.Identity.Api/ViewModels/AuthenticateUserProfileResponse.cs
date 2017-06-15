using System.Web;
using Videpa.Identity.Api.Hypermedia;
using Videpa.Identity.Logic.Models;

namespace Videpa.Identity.Api.ViewModels
{
    public class AuthenticateUserProfileResponse : UserProfileResponse
    {
        public string Jwt { get; set; }
        
        public static AuthenticateUserProfileResponse Build(AuthenticatedUserProfile profile, IRouteFactory routeFactory)
        {
            var vm = new AuthenticateUserProfileResponse
            {
                Jwt = profile.Jwt,
                Name = profile.Name,
                Email = profile.Email
            };

            var profileLike = routeFactory.Link()
                .Route(Routes.UserProfile)
                .Relation(Relations.UserProfile)
                .AddRouteParameter(RouteParameters.UserId, profile.Id)
                .Build();

            vm.Links.Add(profileLike);

            return vm;
        }
    }

    public class UserProfileResponse : LinkedResource
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public static UserProfileResponse Build(UserProfile profile, IRouteFactory routeFactory)
        {
            var vm = new UserProfileResponse
            {
                Name = profile.Name,
                Email = profile.Email
            };

            var profileLike = routeFactory.Link()
                .Route(Routes.UserProfile)
                .Relation(Relations.Self)
                .AddRouteParameter(RouteParameters.UserId, profile.Id)
                .Build();

            vm.Links.Add(profileLike);

            return vm;
        }
    }
}