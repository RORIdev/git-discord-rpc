using System;
using Newtonsoft.Json;

namespace DiscordRichPresence.Net.Entities
{
    public class DiscordActivityTimestamps
    {
        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        protected int? _start { get; set; } = null;

        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        protected int? _end { get; set; } = null;

        [JsonIgnore]
        public DateTimeOffset? StartTime
        {
            get
            {
                if (this._start == null)
                    return null;

                return DateTimeOffset
                    .FromUnixTimeSeconds(this._start.Value);
            }
            set
            {
                if (value == null)
                    this._start = null;
                else
                    this._start = (int)value.Value.ToUnixTimeSeconds();
            }
        }

        [JsonIgnore]
        public DateTimeOffset? EndTime
        {
            get
            {
                if (this._end == null)
                    return null;

                return DateTimeOffset
                    .FromUnixTimeSeconds(this._end.Value);
            }
            set
            {
                if (value == null)
                    this._end = null;
                else
                    this._end = (int)value.Value.ToUnixTimeSeconds();
            }
        }
    }
}