using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using Videpa.Identity.Logic.Exceptions;
using Videpa.Identity.Logic.Ports;

namespace Videpa.Identity.Api.Filters
{
    public class JwtAuthentication : Attribute, IAuthenticationFilter
    {
        public const string FakeSsoToken = "FakeSsoToken";
        private static readonly Regex JwtPrefixRegex = new Regex("^bearer", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() =>
            {
                var authService = FilterHelper.GetInstanceFromDepenencyResolver<IJwtAudience>(context.ActionContext);
                var configuration = FilterHelper.GetInstanceFromDepenencyResolver<IConfigurationManager>(context.ActionContext);

                var jwt = GetJwt(context.Request, configuration);

                var claimsIdentity = authService.Consume(jwt);

                var principal = new ClaimsPrincipal(claimsIdentity);

                context.Principal = principal;

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

        private string GetJwt(HttpRequestMessage request, IConfigurationManager configuration)
        {
            string jwt;
            var jwtHeaderKey = configuration.JwtHeaderKey;
            var fakeIdentity = configuration.FakeIdentity;

            if (fakeIdentity)
            {
                jwt = FakeSsoToken;
            }
            else
            {
                jwt = GetHeaderValue(request, jwtHeaderKey);
                jwt = RemovePrefix(jwt);
            }

            if(jwt == null)
                throw new VidepaAuthorizationException($"A JWT must be placed in the request {jwtHeaderKey}-header value");

            return jwt;
        }

        public static string RemovePrefix(string jwt)
        {
            return JwtPrefixRegex.Replace(jwt, string.Empty).Trim();
        }

        private static string GetHeaderValue(HttpRequestMessage request, string name)
        {
            IEnumerable<string> apiKeyHeaderValues;
            if (request.Headers.TryGetValues(name, out apiKeyHeaderValues))
            {
                return apiKeyHeaderValues.First().Replace("'", "");
            }

            var query = request.RequestUri.ParseQueryString();
            var key = query[name];
            if (!string.IsNullOrEmpty(key))
            {
                return key.Replace("'", "");
            }

            return string.Empty;
        }
    }
}