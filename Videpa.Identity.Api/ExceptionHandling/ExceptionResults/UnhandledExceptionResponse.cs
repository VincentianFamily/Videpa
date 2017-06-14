using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Videpa.Identity.Api.ExceptionHandling.ExceptionResults
{
    public class UnhandledExceptionResponse : IHttpActionResult
    {
        private readonly Exception _exception;
        private readonly HttpRequestMessage _request;
        private readonly Guid _requestId;

        public UnhandledExceptionResponse(HttpRequestMessage requestMessage, Exception exception, Guid requestId)
        {
            _exception = exception;
            _request = requestMessage;
            _requestId = requestId;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            // var showException = _configurationManager.DisplayStackTraceInErrorResponse;

            var error = new
            {
                RequestId = _requestId.ToString(),
                Message = "Unhandled Exception - please contact our API support with the requestId as reference",
                // StackTrace = showException ? _exception : null
            };

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = ExceptionResponseHelper.SerializeContent(error),
                RequestMessage = _request
            };

            return Task.FromResult(response);
        }
    }
}