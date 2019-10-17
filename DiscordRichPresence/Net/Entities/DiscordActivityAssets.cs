using Newtonsoft.Json;

namespace DiscordRichPresence.Net.Entities
{
    public class DiscordActivityAssets
    {
        [JsonProperty("large_image", NullValueHandling = NullValueHandling.Ignore)]
        public string LargeImage { get; set; } = null;

        [JsonProperty("large_text", NullValueHandling = NullValueHandling.Ignore)]
        public string LargeText { get; set; } = null;

        [JsonProperty("small_image", NullValueHandling = NullValueHandling.Ignore)]
        public string SmallImage { get; set; } = null;

        [JsonProperty("small_text", NullValueHandling = NullValueHandling.Ignore)]
        public string SmallText { get; set; } = null;
    }
}