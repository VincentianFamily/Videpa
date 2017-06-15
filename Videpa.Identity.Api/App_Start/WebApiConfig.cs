using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Videpa.Identity.Api.Hypermedia;

namespace Videpa.Identity.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API attribute routes
            // config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: Routes.AuthenticateUserProfile,
               routeTemplate: $"api/login",
               defaults: new { controller = "UserProfile", action = "Authenticate" }
               );

            config.Routes.MapHttpRoute(
               name: Routes.CreateUserProfile,
               routeTemplate: $"api/users",
               defaults: new { controller = "UserProfile", action = "CreateProfile" }
               );

            config.Routes.MapHttpRoute(
                name: Routes.UserProfile,
                routeTemplate: "api/users/{"+RouteParameters.UserId + "}",
                defaults: new { controller = "UserProfile", action = "Profile" }
                );

            config.Routes.MapHttpRoute(
                name: Routes.UserProfile,
                routeTemplate: "api/users/{" + RouteParameters.UserId + "}/change-email",
                defaults: new { controller = "UserProfile", action = "ChangeEmail" }
                );

            config.Routes.MapHttpRoute(
                name: Routes.UserProfile,
                routeTemplate: "api/users/{" + RouteParameters.UserId + "}/change-password",
                defaults: new { controller = "UserProfile", action = "ChangePassword" }
                );

            // 404 end point
            config.Routes.MapHttpRoute( // A catch-all for URL mistakes
                name: "404",
                routeTemplate: "api/{*url}",
                defaults: new { controller = "Error", action = "NotFound" }
            );

            // Remove XML formatter - we <3 JSON
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Add camelCase formatter
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
