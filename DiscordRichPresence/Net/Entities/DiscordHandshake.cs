using System.Globalization;
using Newtonsoft.Json;

namespace DiscordRichPresence.Net.Entities
{
    public sealed class DiscordHandshake
    {
        [JsonProperty("v")]
        public int Version { get; private set; } = 1;

        [JsonProperty("client_id")]
        private string _ClientId;

        [JsonIgnore]
        public ulong ClientId
        {
            get => ulong.Parse(this._ClientId);
            set => this._ClientId = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
