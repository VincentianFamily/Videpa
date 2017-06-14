using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Videpa.Identity.Api.Hypermedia;
using Videpa.Identity.Api.Routing;
using Videpa.Identity.Api.ViewModels;

namespace Videpa.Identity.Api.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IHttpActionResult NoContent()
        {
            return new NoContentResult(Request);
        }

        /// <summary>
        /// 201 Created with Self-link as location header
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        protected CreatedNegotiatedContentResult<T> Created<T>(T content) where T : LinkedResource
        {
            return Created(ExtractLocationFromLinkedResource(content), content);
        }

        private string ExtractLocationFromLinkedResource(LinkedResource resource)
        {
            var selfLink = resource.Links.FirstOrDefault(p => p.Rel == Relations.Self);

            if (selfLink == null)
                return string.Empty;

            return selfLink.Href;
        }
    }

    public class NoContentResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly string _reason;

        public NoContentResult(HttpRequestMessage request, string reason)
        {
            _request = request;
            _reason = reason;
        }

        public NoContentResult(HttpRequestMessage request)
        {
            _request = request;
            _reason = "No Content";
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.NoContent, _reason);
            return Task.FromResult(response);
        }
    }
}