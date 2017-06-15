using System;

namespace Videpa.Identity.Api.Hypermedia
{
    public interface ILinkBuilder
    {
        ILinkBuilder Relation(string relation);
        ILinkBuilder Route(string route);
        
        /// <summary>
        /// Default method is get. Only Put, Post, Patch should include a template
        /// </summary>
        /// <param name="method">The default method is get</param>
        /// <param name="template">Post should include a template type, which should match the request model supplied by the client</param>
        /// <returns></returns>
        ILinkBuilder Method(string method = HttpMethods.Get, Type template = null);
        
        /// <summary>
        /// Adds a body section to the link which must be send with the request
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        ILinkBuilder Body(object body);

        ILinkBuilder AddRouteParameter(string key, object value);
        
        /// <summary>
        /// Adds a RouteParameter with the value represented as a placeholder: {key}
        /// </summary>
        /// <param name="key">Name of the route parameter</param>
        /// <returns></returns>
        ILinkBuilder AddRouteParameterAsPlaceholder(string key);
        ILinkBuilder AddQueryStringParameter(string key, object value);
        
        ILinkBuilder Clone();
        ILinkBuilder Comment(string comment);
        ILinkBuilder MarkAsObsolete();
        Link Build();
    }
}