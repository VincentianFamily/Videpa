using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Videpa.Identity.Api.ExceptionHandling.ExceptionResults
{
    public static class ExceptionResponseHelper
    {
        public static StringContent SerializeContent(object payload)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new JsonConverter[] { new IsoDateTimeConverter() }
            };

            var stringErrorMessage = JsonConvert.SerializeObject(payload, settings);

            var content = new StringContent(stringErrorMessage, Encoding.UTF8, "application/json");

            return content;
        }
    }
}