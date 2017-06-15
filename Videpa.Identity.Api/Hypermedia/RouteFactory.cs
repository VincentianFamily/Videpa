using System.Web.Http.Routing;

namespace Videpa.Identity.Api.Hypermedia
{
    public class RouteFactory : IRouteFactory
    {
        private readonly UrlHelper _urlHelper;

        public RouteFactory(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ILinkBuilder Link()
        {
            return new LinkBuilder(_urlHelper);
        }
    }
}