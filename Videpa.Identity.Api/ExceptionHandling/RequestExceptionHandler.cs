using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Videpa.Core.Exceptions;
using Videpa.Identity.Api.ExceptionHandling.ExceptionResults;

namespace Videpa.Identity.Api.ExceptionHandling
{
    public class RequestExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            context.Result = ExceptionResponseFactory(context.Request, context.Exception);

            return Task.FromResult(default(AsyncVoid));
        }

        private struct AsyncVoid
        {
        }

        /// <summary>
        /// Evaluates exception and returns a response message
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static IHttpActionResult ExceptionResponseFactory(HttpRequestMessage requestMessage, Exception exception)
        {
            var ex = exception as VidepaException;

            if (ex != null)
            {
                return new ExceptionResponse(requestMessage, ex);
            }

            return new UnhandledExceptionResponse(requestMessage, exception, Guid.Empty);
        }

    }
}