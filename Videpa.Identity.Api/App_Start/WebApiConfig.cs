using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Videpa.Identity.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API attribute routes
            // config.MapHttpAttributeRoutes();



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
