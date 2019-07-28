using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities.Configuration
{
    public partial class AssetsConfig
    {
        [JsonProperty("extensions")]
        protected List<DiscordAsset> Extensions { get; private set; } = new List<DiscordAsset>();

        public void Add(DiscordAsset asset)
        {
            lock (Extensions)
            {
                var index = Extensions.FindIndex(x => x.Key == asset.Key);

                if (index != -1)
                    Extensions.RemoveAt(index);

                Extensions.Add(asset);
            }
        }

        public void Remove(DiscordAsset asset)
        {
            lock (Extensions)
                Extensions.Remove(asset);
        }

        public DiscordAsset this[string name]
        {
            get
            {
                lock (Extensions)
                {
                    var index = this.Extensions.FindIndex(x => x.Key == name || x.Extensions.Any(y => y == name));

                    if (index == -1)
                        return DiscordAsset.Empty;

                    return this.Extensions[index];
                }
            }
            set
            {
                if(value != null)
                    Add(value);
            }
        }
    }

    public class DiscordAsset : IEquatable<DiscordAsset>
    {
        public static readonly DiscordAsset Empty = new DiscordAsset();

        public DiscordAsset() { }

        public DiscordAsset(string key, string value, params string[] extensions)
        {
            this.Key = key;
            this.Value = value;
            this.Extensions = extensions;
        }

        [JsonProperty("key")]
        public string Key { get; private set; } = "default_file";

        [JsonProperty("value")]
        public string Value { get; private set; } = "Unknown File";

        [JsonProperty("extensions")]
        public IEnumerable<string> Extensions { get; private set; } = new[] { "*" };

        public bool Equals(DiscordAsset other)
        {
            return this.Key == other?.Key;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return Equals(obj as DiscordAsset);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + this.Key.GetHashCode();
                hash = hash * 32 + this.Extensions.GetHashCode();
                return hash;
            }
        }
    }

    [Flags]
    public enum AssetType : byte
    {
        Small,
        Large
    }
}
