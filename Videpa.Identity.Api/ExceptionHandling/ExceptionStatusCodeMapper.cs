using System.Net;
using Videpa.Core.Exceptions;
using Videpa.Identity.Logic.Exceptions;

namespace Videpa.Identity.Api.ExceptionHandling
{
    public class ExceptionStatusCodeMapper
    {
        public static HttpStatusCode Map(VidepaException ex)
        {
            if (ex is VidepaAuthenticationException || ex is VidepaAuthorizationException)
                return HttpStatusCode.Unauthorized;

            if (ex is VidepaNotFoundException)
                return HttpStatusCode.NotFound;

            return HttpStatusCode.BadRequest;
        }
    }
}