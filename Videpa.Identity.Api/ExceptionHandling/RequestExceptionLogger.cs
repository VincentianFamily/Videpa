using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace Videpa.Identity.Api.ExceptionHandling
{
    public class RequestExceptionLogger : ExceptionLogger
    {
        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var ipAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "0.0.0.0";
            return Task.Run(() => LogException(context.Exception, ipAddress), cancellationToken);
        }

        internal void LogException(Exception exception, string ipAddress)
        {
            // _logger.Error(exception.Message, exception);
        }
    }
}
