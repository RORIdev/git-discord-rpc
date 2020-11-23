using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public delegate string GetStringFormatDelegate(string key, params object[] args);

    public class LocalizationSettings
    {
        [JsonIgnore]
        public static readonly LocalizationSettings Empty = new LocalizationSettings
        {
            Data = new List<LocalizationEntry>
            {
                new LocalizationEntry("base_working_text", "Working On: {0}"),
                new LocalizationEntry("base_solution_text", "Solution: {0}"),
                new LocalizationEntry("base_editing_text", "Editing: {0}"),
                new LocalizationEntry("base_unknown_file_text", "Unknown File")
            }
        };

        [JsonProperty("strings")]
        protected List<LocalizationEntry> Data { get; set; } = new List<LocalizationEntry>();

        public GetStringFormatDelegate GetFormatDelegate()
        {
            return new GetStringFormatDelegate((key, args) =>
            {
                if (!this.FindString(key, out var str))
                    return key;

                return str.Format(args);
            });
        }

        public bool FindString(string key, out LocalizationEntry value)
        {
            value = null;

            if (string.IsNullOrEmpty(key))
                return false;

            lock (this.Data)
            {
                var index = this.Data.FindIndex(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant());

                if (index == -1)
                    return false;

                value = this.Data[index];
                return true;
            }
        }

        public bool Add(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;

            lock (this.Data)
            {
                var index = this.Data.FindIndex(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant());

                if (index == -1)
                    this.Data.Add(new LocalizationEntry { Key = key, Text = value });
                else
                {
                    this.Data.RemoveAt(index);
                    this.Data.Insert(index, new LocalizationEntry { Key = key, Text = value });
                }

                return true;
            }
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            lock (this.Data)
            {
                var index = this.Data.FindIndex(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant());

                if (index == -1)
                    return false;

                this.Data.RemoveAt(index);
                return true;
            }
        }
    }
}