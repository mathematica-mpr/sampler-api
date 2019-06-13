using Newtonsoft.Json;

namespace sampler_api.Models
{
    public class SimulateHttpResponse
    {
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("multiValueHeaders")]
        public string MulitiValueHeaders { get; set; }

        [JsonProperty("headers")]
        public Headers Headers { get; set; }
    }

    public class Headers
    {
        [JsonProperty("Access-Control-Allow-Origin")]
        public string AccessControlAllowOrigin { get; set; }
    }
}