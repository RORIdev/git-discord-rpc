using Newtonsoft.Json;

namespace DiscordRichPresence.Net.Entities
{
    public class DiscordActivitySecrets
    {
        [JsonProperty("join", NullValueHandling = NullValueHandling.Ignore)]
        public string Join { get; set; }

        [JsonProperty("spectate", NullValueHandling = NullValueHandling.Ignore)]
        public string Spectate { get; set; }

        [JsonProperty("match", NullValueHandling = NullValueHandling.Ignore)]
        public string Match { get; set; }
    }
}