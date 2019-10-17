using System.Globalization;
using Newtonsoft.Json;

namespace DiscordRichPresence.Net.Entities
{
    public sealed class DiscordUser
    {
        [JsonProperty("discriminator")]
        protected string _discriminator;

        [JsonProperty("id")]
        public ulong Id { get; internal set; }

        [JsonProperty("username")]
        public string Username { get; internal set; }

        [JsonIgnore]
        public int Discriminator
        {
            get => int.Parse(this._discriminator, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        [JsonProperty("avatar")]
        public string AvatarHash { get; internal set; }

        [JsonIgnore]
        public string AvatarUrl
            => !string.IsNullOrWhiteSpace(this.AvatarHash) ? (this.AvatarHash.StartsWith("a_") ? $"https://cdn.discordapp.com/avatars/{this.Id.ToString(CultureInfo.InvariantCulture)}/{this.AvatarHash}.gif?size=1024" : $"https://cdn.discordapp.com/avatars/{this.Id}/{this.AvatarHash}.png?size=1024") : this.DefaultAvatarUrl;

        [JsonIgnore]
        public string DefaultAvatarUrl
            => $"https://cdn.discordapp.com/embed/avatars/{(this.Discriminator % 5).ToString(CultureInfo.InvariantCulture)}.png?size=1024";

        [JsonProperty("bot")]
        public bool IsBot { get; internal set; } = false;

        public override string ToString()
        {
            return $"DiscordUser ({this.Username}#{this.Discriminator})";
        }
    }
}
