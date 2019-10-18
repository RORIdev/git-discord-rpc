using System;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiscordRichPresence.Pipe
{
    public sealed class DiscordFrame
    {
        public DiscordFrameType Type { get; internal set; } = DiscordFrameType.Frame;
        public JObject Payload { get; internal set; } = new JObject();

        internal byte[] GetBytes()
        {
            var data = Encoding.UTF8.GetBytes(this.Payload.ToString(Formatting.None));
            var type = BitConverter.GetBytes((int)this.Type);
            var length = BitConverter.GetBytes(data.Length);

            var result = new byte[sizeof(int) * 2 + data.Length];
            Array.Copy(type, 0, result, 0, type.Length);
            Array.Copy(length, 0, result, 4, length.Length);
            Array.Copy(data, 0, result, 8, data.Length);
            return result;
        }

        public DiscordFrame()
        {

        }

        public DiscordFrame WithType(DiscordFrameType type)
        {
            this.Type = type;
            return this;
        }

        public DiscordFrame WithPayload(object payload)
        {
            this.Payload = JObject.FromObject(payload);
            return this;
        }

        public DiscordFrame(byte[] raw)
        {
            byte[] payload;
            DiscordFrameType type;
            {
                var btype = new byte[sizeof(int)];
                var blength = new byte[sizeof(int)];

                Array.Copy(raw, 0, btype, 0, btype.Length);
                Array.Copy(raw, 4, blength, 0, blength.Length);

                type = (DiscordFrameType)BitConverter.ToInt32(btype, 0);
                var length = BitConverter.ToInt32(blength, 0);

                payload = new byte[length];
                Array.Copy(raw, 8, payload, 0, length);
            }
            this.Type = type;
            this.Payload = JObject.Parse(Encoding.UTF8.GetString(payload));
        }

        internal string GetJson()
        {
            Debug.WriteLine("DiscordFrame::GetJson() called.");
            return this.Payload.ToString(Formatting.None);
        }
    }
}
