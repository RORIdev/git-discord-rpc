using Newtonsoft.Json;

namespace DiscordRichPresence.Pipe.Entities
{
    public class DiscordActivityParty
    {
        void CheckSizeNull()
        {
            lock (this)
                if (this._size == null) this._size = new int[2] { 0, 0 };
        }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        protected int[] _size = null;

        [JsonIgnore]
        public int Size
        {
            get
            {
                this.CheckSizeNull();
                return this._size[0];
            }
            set
            {
                this.CheckSizeNull();
                this._size[0] = value;
            }
        }

        [JsonIgnore]
        public int Max
        {
            get
            {
                this.CheckSizeNull();
                return this._size[1];
            }
            set
            {
                this.CheckSizeNull();
                this._size[1] = value;
            }
        }
    }
}