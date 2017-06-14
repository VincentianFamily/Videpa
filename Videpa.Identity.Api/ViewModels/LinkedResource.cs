using System.Collections.Generic;
using Newtonsoft.Json;
using Videpa.Identity.Api.Hypermedia;

namespace Videpa.Identity.Api.ViewModels
{
    public class LinkedResource
    {
        public LinkedResource()
        {
            Links = new List<Link>();
        }

        [JsonProperty(Order = 1, PropertyName = "_links")]
        public List<Link> Links { get; set; }
    }
}