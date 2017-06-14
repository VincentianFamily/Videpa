using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;
using Videpa.Identity.Api.Hypermedia;

namespace Videpa.Identity.Api.Routing
{
    public class LinkBuilder : ILinkBuilder
    {
        public const string ObsoleteEndpoint = "[Obsolete] This endpoint is deprecated and will removed";

        private readonly UrlHelper _urlHelper;
        private string _route;
        private string _method;
        private Type _template;
        private string _relation;
        private string _comment;
        private object _body;
        private readonly Dictionary<string, object> _routeValues;
        private readonly Dictionary<string, object> _queryStringValues;
        private readonly Dictionary<string, string> _routePalceholders;

        public LinkBuilder(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            _routeValues = new Dictionary<string, object>();
            _queryStringValues = new Dictionary<string, object>();
            _routePalceholders = new Dictionary<string, string>();
        }

        public ILinkBuilder Relation(string relation)
        {
            _relation = relation;
            return this;
        }

        public ILinkBuilder Route(string route)
        {
            _route = route;
            return this;
        }

        public ILinkBuilder Method(string method, Type template = null)
        {
            _method = method;
            _template = template;
            return this;
        }

        public ILinkBuilder Comment(string comment)
        {
            _comment = comment;
            return this;
        }

        public ILinkBuilder MarkAsObsolete()
        {
            _comment = ObsoleteEndpoint;
            return this;
        }

        public ILinkBuilder Body(object body)
        {
            _body = body;
            return this;
        }

        public ILinkBuilder AddRouteParameter(string key, object value)
        {
            _routeValues[key] = value;
            return this;
        }

        public ILinkBuilder AddRouteParameterAsPlaceholder(string key)
        {
            var value = string.Format("--{0}--", key); // format with dash since other characters are Uri encoded
            _routePalceholders[key] = value; // Used to replace later
            _routeValues[key] = value;
            return this;
        }

        public ILinkBuilder AddQueryStringParameter(string key, object value)
        {
            if (value is DateTime)
                value = ((DateTime)value).ToString("yyyy-MM-dd");

            if (value is DateTimeOffset)
                value = ((DateTimeOffset)value).ToString("yyyy-MM-dd");

            _queryStringValues[key] = value;
            return this;
        }

        public ILinkBuilder Clone()
        {
            var linkBuilder = new LinkBuilder(_urlHelper).Route(_route).Relation(_relation);

            if (!string.IsNullOrWhiteSpace(_method))
                linkBuilder.Method(_method, _template);

            foreach (var routeValue in _routeValues)
                linkBuilder.AddRouteParameter(routeValue.Key, routeValue.Value);

            foreach (var routeValue in _queryStringValues)
                linkBuilder.AddQueryStringParameter(routeValue.Key, routeValue.Value);

            return linkBuilder;
        }

        public Link Build()
        {
            if (_urlHelper == null)
                throw new ArgumentNullException("UrlHelper is not set to an instance of UrlHelper");

            if (string.IsNullOrWhiteSpace(_route))
                throw new ArgumentNullException("No route has been set in RelationUrl");

            if (string.IsNullOrWhiteSpace(_relation))
                throw new ArgumentNullException("No relation has been set in RelationUrl");

            var qs = QueryString();

            var href = _urlHelper.Link(_route, _routeValues) + qs;

            if (_routePalceholders.Any()) // Replaces placeholders --key-- with {key} 
                href = FormatPlaceholders(href, _routePalceholders);
            
            return new Link(_relation, href, _method, _body, _comment);
        }

        private string FormatPlaceholders(string href, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
                href = href.Replace(placeholder.Value, $"{{{placeholder.Key}}}");

            return href;
        }
        
        private string QueryString()
        {
            if (!_queryStringValues.Any())
                return string.Empty;

            return "?" + string.Join("&", _queryStringValues.Select(p => p.Key + "=" + p.Value));
        }

        Link ILinkBuilder.Build()
        {
            throw new NotImplementedException();
        }
    }
}