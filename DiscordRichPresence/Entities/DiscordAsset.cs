using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class DiscordAsset
    {
        /// <summary>
        /// Chave do asset.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; } = "default_file";

        /// <summary>
        /// Determina se o asset é um asset padrão de arquivo desconhecido.
        /// </summary>
        [JsonProperty("is_default", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Texto que será exibido no asset.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; } = "#unknown_file";

        /// <summary>
        /// Extensões que são aplicaveis no asset.
        /// </summary>
        [JsonProperty("extensions", NullValueHandling = NullValueHandling.Ignore)]
        protected List<string> Extensions { get; set; } = new List<string>();
    }
}
