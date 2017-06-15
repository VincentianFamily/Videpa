using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using SimpleInjector;

namespace Videpa.Identity.Api.ExceptionHandling
{
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