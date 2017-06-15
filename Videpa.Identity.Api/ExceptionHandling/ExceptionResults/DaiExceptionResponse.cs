using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Videpa.Core.Exceptions;

namespace Videpa.Identity.Api.ExceptionHandling.ExceptionResults
{
    public class ExceptionResponse : IHttpActionResult 
    {
        public HttpRequestMessage Request { get; private set; }
        public VidepaException Exception { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }

        public ExceptionResponse(HttpRequestMessage request, VidepaException exception)
        {
            Request = request;
            Exception = exception;

            StatusCode = ExceptionStatusCodeMapper.Map(exception);
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var error = BuildErrorResponseMessage();
            
            var response = new HttpResponseMessage(StatusCode)
            {
                Content = ExceptionResponseHelper.SerializeContent(error), 
                RequestMessage = Request,
                
            };

            if (Headers != null)
            {
                foreach (var header in Headers)
                    response.Headers.Add(header.Key, header.Value);
            }
            
            return Task.FromResult(response);
        }

        protected ErrorResponseMessage BuildErrorResponseMessage()
        {
            var msg = new ErrorResponseMessage
            {
                Time = DateTime.UtcNow,
                RequestId = Guid.Empty.ToString(),
                HttpStatusCode = (int)StatusCode,
                Message = Exception.Message,
                DeveloperHints = "TODO!",
            };
            
            return msg;
        }
    }
}