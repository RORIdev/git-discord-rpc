using Newtonsoft.Json;

namespace DiscordRichPresence.Pipe.Entities
{
    public sealed class DiscordConfig
    {
        [JsonProperty("cdn_host")]
        public string CdnHost { get; internal set; }

        [JsonProperty("api_endpoint")]
        public string ApiEndpoint { get; internal set; }

        [JsonProperty("environment")]
        public string Environment { get; internal set; }
    }
}
