using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class RichPresenceConfiguration
    {
        [JsonProperty("assets")]
        protected List<RichPresenceAsset> Assets { get; set; } = new List<RichPresenceAsset>();

        public void Add(RichPresenceAsset asset)
        {
            if (asset == null)
                return;

            if (string.IsNullOrEmpty(asset.Key))
                return;

            lock(this.Assets)
            {
                var index = this.Assets.FindIndex(x => x.Key.Equals(asset.Key));

                if (index != -1)
                    this.Assets.RemoveAt(index);

                this.Assets.Add(asset);
            }
        }

        public void Remove(RichPresenceAsset asset)
        {
            if (asset == null)
                return;

            if (string.IsNullOrEmpty(asset.Key))
                return;

            lock (this.Assets)
            {
                var index = this.Assets.FindIndex(x => x.Key.Equals(asset.Key));

                if (index != -1)
                    return;

                this.Assets.RemoveAt(index);
            }
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                return;

            lock (this.Assets)
            {
                var index = this.Assets.FindIndex(x => x.Key.Equals(key));

                if (index != -1)
                    return;

                this.Assets.RemoveAt(index);
            }
        }
    }
}
