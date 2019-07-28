using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordRichPresence.Entities.Configuration;
using Newtonsoft.Json;

namespace DiscordRichPresence
{
    public class DiscordConfigManager
    {
        private DiscordConfig _discord;
        private AssetsConfig _assets;

        public DiscordConfigManager()
        {
            _discord = new DiscordConfig();
            _assets = new AssetsConfig();
        }

        public DiscordConfig Discord => _discord;
        public AssetsConfig Assets => _assets;

        public void Initialize()
        {
            _discord = GetOrCreate("discord", DiscordConfig.Default);
            _assets = GetOrCreate("assets", AssetsConfig.Default);
        }

        private T GetOrCreate<T>(string name, T @default) where T : class
        {
            var env = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var file = new FileInfo(Path.Combine(env, "My Games", "Visual Studio Rich Presence", $"{name}.json"));

            if (!file.Directory.Exists)
                file.Directory.Create();

            var obj = @default;

            if(!file.Exists)
            {
                using (var sw = file.CreateText())
                {
                    sw.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
                    sw.Flush();
                }
            }
            else
            {
                try
                {
                    using (var sr = file.OpenText())
                        obj = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), $"Discord Rich Presence: Failed to load config {name}...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return obj;
        }
    }
}
