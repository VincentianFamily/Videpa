using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
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

            return routeValue.ToString();
        }

        public static string GetRouteParameterValue(HttpActionContext actionContext, string routeParameter)
        {
            var obj = actionContext.RequestContext.RouteData.Values.GetValueOrDefault(routeParameter);

            if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
                throw new VidepaNotFoundException($"RouteParameter is missing: {routeParameter}");

            return obj.ToString();
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

    public class JwtAuthentication : Attribute, IAuthenticationFilter
    {
        public const string FakeSsoToken = "FakeSsoToken";
        private static readonly Regex JwtPrefixRegex = new Regex("^bearer", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() =>
            {
                // TODO Set JWT - how to handle async

                //var authService = FilterHelper.GetInstanceFromDepenencyResolver<IAuthService>(context.ActionContext);
                //var configuration = FilterHelper.GetInstanceFromDepenencyResolver<IConfigurationManager>(context.ActionContext);

                //var jwt = GetJwt(context.Request, configuration);

                //var claimsIdentity = authService.AuthenticateUserFromJwt(jwt);

                //var principal = IdentityService.Set(claimsIdentity);

                //if (HttpContext.Current != null)
                //    HttpContext.Current.User = principal;

                //context.Principal = principal;
            }, cancellationToken);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            // Add authentication challenge to response
            return Task.FromResult(0);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }

        private string GetJwt(HttpRequestMessage request)
        {
            //string ssoToken;
            //var key = configuration.SsoTokenName;
            //var fakeSso = configuration.FakeSso;

            //if (fakeSso)
            //{
            //    ssoToken = FakeSsoToken;
            //}
            //else
            //{
            //    ssoToken = GetKey(request, key);
            //    ssoToken = RemovePrefix(ssoToken);
            //}

            //if(ssoToken == null)
            //    throw new VidepaAuthorizationException("No jwt is supplied");

            return Guid.Empty.ToString();
        }

        public static string RemovePrefix(string jwt)
        {
            return JwtPrefixRegex.Replace(jwt, string.Empty).Trim();
        }

        private static string GetKey(HttpRequestMessage request, string name)
        {
            IEnumerable<string> apiKeyHeaderValues;
            if (request.Headers.TryGetValues(name, out apiKeyHeaderValues))
            {
                return apiKeyHeaderValues.First().Replace("'", "");
            }

            var query = request.RequestUri.ParseQueryString();
            string key = query[name];
            if (!string.IsNullOrEmpty(key))
            {
                return key.Replace("'", "");
            }

            return string.Empty;
        }
    }
}