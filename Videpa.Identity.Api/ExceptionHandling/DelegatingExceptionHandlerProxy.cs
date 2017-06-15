using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using SimpleInjector;

namespace Videpa.Identity.Api.ExceptionHandling
{
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
}