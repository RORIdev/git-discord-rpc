using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class RichPresenceAsset
    {
        [JsonProperty("key")]
        public string Key { get; set; } = "default_file";

        [JsonProperty("is_default", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsDefault { get; set; } = false;

        [JsonProperty("text")]
        public string Text { get; set; } = "#unknown_file_desc";

        [JsonProperty("extensions", NullValueHandling = NullValueHandling.Ignore)]
        protected List<string> Extensions { get; set; } = new List<string>();
    }
}
