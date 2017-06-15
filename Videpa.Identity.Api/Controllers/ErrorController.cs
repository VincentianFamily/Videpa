using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Videpa.Identity.Api.ExceptionHandling.ExceptionResults;
using Videpa.Identity.Logic.Exceptions;

namespace Videpa.Identity.Api.Controllers
{
    public class ErrorController : ApiController
    {
        [HttpGet]
        [ActionName("NotFound")]
        public HttpResponseMessage Get()
        {
            return BuildErrorMessage();
        }

        [HttpPost]
        [ActionName("NotFound")]
        public HttpResponseMessage Post()
        {
            return BuildErrorMessage();
        }

        private HttpResponseMessage BuildErrorMessage()
        {
            var exception = new VidepaNotFoundException("The endpoint could not be found. You might be missing a required part of the URL or maybe the URL violates a constraint.");
            var msg = new ErrorResponseMessage
            {
                Time = DateTimeOffset.Now,
                HttpStatusCode = (int)HttpStatusCode.NotFound,
                Message = exception.Message
            };

            var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, msg.Message);
            response.Content = ExceptionResponseHelper.SerializeContent(msg);

            return response;
        }
    }
}