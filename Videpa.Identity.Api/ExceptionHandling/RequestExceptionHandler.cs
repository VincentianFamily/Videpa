using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using SimpleInjector;
using Videpa.Core.Exceptions;
using Videpa.Identity.Api.ExceptionHandling.ExceptionResults;

namespace Videpa.Identity.Api.ExceptionHandling
{
    public class RequestExceptionHandler : IExceptionHandler
    {
        public RequestExceptionHandler()
        {
        }

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

    public sealed class DelegatingExceptionHandlerProxy<THandler> : IExceptionHandler where THandler : class, IExceptionHandler
    {
        private readonly Container _container;

        public DelegatingExceptionHandlerProxy(Container container)
        {
            _container = container;
        }

        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            context.Request.GetDependencyScope();

            var handler = _container.GetInstance<THandler>();

            return handler.HandleAsync(context, cancellationToken);
        }
    }

    public sealed class DelegatingExceptionLoggerProxy<THandler> : ExceptionLogger where THandler : ExceptionLogger
    {
        private readonly Container _container;

        public DelegatingExceptionLoggerProxy(Container container)
        {
            _container = container;
        }

        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            context.Request.GetDependencyScope();
            var handler = _container.GetInstance<THandler>();
            return handler.LogAsync(context, cancellationToken);
        }
    }
}