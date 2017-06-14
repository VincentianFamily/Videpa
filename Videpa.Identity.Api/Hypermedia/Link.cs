using System;
using Newtonsoft.Json;

namespace Videpa.Identity.Api.Hypermedia
{
    public class Link
    {
        public Link(string rel, string href, string method = null, object body = null, string comment = null)
        {
            if (string.IsNullOrWhiteSpace(rel))
                throw new ArgumentNullException("rel", $"No relation value in Link.Ctor (href: {href ?? "null"})");

            if (string.IsNullOrWhiteSpace(href))
                throw new ArgumentNullException("href", $"No href value in Link.Ctor (rel: {rel})");

            Rel = rel;
            Href = href;
            Method = method;
            Body = body;
            Comment = comment;
        }

        public string Rel { get; private set; }
        public string Href { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Body { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; private set; }
    }
}