using DiscordRichPresence.Pipe.Entities;
using Newtonsoft.Json;

namespace DiscordRichPresence.Pipe.EventArgs
{
    public sealed class ReadyEventArgs : System.EventArgs
    {
        [JsonIgnore]
        public DiscordPipeClient Client { get; internal set; }

        [JsonProperty("v")]
        public int Version { get; internal set; }

        [JsonProperty("config")]
        public DiscordConfig Configuration { get; internal set; }

        [JsonProperty("user")]
        public DiscordUser User { get; internal set; }
    }
}
