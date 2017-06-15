using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using Videpa.Core;
using Videpa.Identity.Logic.Exceptions;

namespace Videpa.Identity.Api.Filters
{
    public static class FilterHelper
    {
        /// <summary>
        /// Gets service instance from the depenedency resolver associated to the current controller context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static T GetInstanceFromDepenencyResolver<T>(HttpActionContext actionContext)
        {
            var resolver = actionContext.ControllerContext.Configuration.DependencyResolver;
            var instance = resolver.GetService(typeof(T));
            return (T)instance;
        }

        /// <summary>
        /// Extracts the either a route parameter value or a querystring value from the requests Url
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="routeParameter"></param>
        /// <returns></returns>
        public static string GetUrlParameter(HttpActionContext actionContext, string routeParameter)
        {
            var routeValue = actionContext.RequestContext.RouteData.Values.GetValueOrDefault(routeParameter);

            if (routeValue == null) // check querystring (not case sensitive)
            {
                var lowerParameter = routeParameter.ToLower();

                var queryString = actionContext.Request
                    .GetQueryNameValuePairs()
                    .ToDictionary(x => x.Key.ToLower(), x => x.Value);

                if (queryString.ContainsKey(lowerParameter))
                    routeValue = queryString[lowerParameter];
            }

            if (routeValue == null)
                throw new VidepaArgumentException($"No {routeParameter} is supplied");

            return HttpUtility.UrlDecode(routeValue.ToString());
        }

        public static string GetRouteParameterValue(HttpActionContext actionContext, string routeParameter)
        {
            var obj = actionContext.RequestContext.RouteData.Values.GetValueOrDefault(routeParameter);

            if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
                throw new VidepaNotFoundException($"RouteParameter is missing: {routeParameter}");

            return HttpUtility.UrlDecode(obj.ToString());
        }

        /// <summary>
        /// Returns key-value pairs from the query string. All keys are lower-case.
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryStringKeyValues(HttpActionContext actionContext)
        {
            var keyValues = actionContext.Request
                .GetQueryNameValuePairs()
                .ToDictionary(x => x.Key.ToLower(), x => x.Value);

            return keyValues;
        }
    }
}