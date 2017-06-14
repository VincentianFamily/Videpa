using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Videpa.Identity.Api.Hypermedia;

namespace Videpa.Identity.Api.ViewModels
{
    public class LinkedResourceCollection<T> where T : class
    {
        public LinkedResourceCollection()
        {
            Links = new List<Link>();
            Collection = new List<T>();
        }

        public LinkedResourceCollection(IEnumerable<T> entities)
        {
            Links = new List<Link>();
            Collection = entities.ToList();
        }

        public List<T> Collection { get; set; }

        [JsonProperty(PropertyName = "_links")]
        public List<Link> Links { get; set; }
    }
}