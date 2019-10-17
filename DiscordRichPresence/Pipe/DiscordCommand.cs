using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace DiscordRichPresence.Pipe
{
    public class DiscordCommand
    {
        [JsonProperty("cmd"), JsonConverter(typeof(StringEnumConverter))]
        public DiscordCommandType Command { get; internal set; }

        [JsonProperty("args", NullValueHandling = NullValueHandling.Ignore)]
        public JObject Arguments { get; internal set; } = null;

        [JsonProperty("nonce")]
        public string Nonce { get; internal set; } = Guid.NewGuid().ToString();

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public JObject Data { get; internal set; } = null;

        [JsonProperty("evt", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(StringEnumConverter))]
        public DiscordEventType? Event { get; internal set; } = null;
    }
}
