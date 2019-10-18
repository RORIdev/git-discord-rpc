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
        public string Text { get; set; }

        /// <summary>
        /// Extensões que são aplicaveis no asset.
        /// </summary>
        [JsonProperty("extensions", NullValueHandling = NullValueHandling.Ignore)]
        protected List<string> Extensions { get; set; } = new List<string>();

        /// <summary>
        /// Verifica o suporte da extensão pelo asset atual.
        /// </summary>
        /// <param name="ext">Extensão que será verificada</param>
        /// <returns>Retorna se é a extensão é suportada.</returns>
        public bool SupportExtension(string ext)
        {
            lock (this.Extensions)
            {
                var index = this.Extensions.FindIndex(x => x.ToLowerInvariant() == ext.ToLowerInvariant());
                return index != -1;
            }
        }

        /// <summary>
        /// Adiciona a extensão no asset atual;
        /// </summary>
        /// <param name="ext">Extensão que será adicionada.</param>
        public void AddExtension(string ext)
        {
            lock (this.Extensions)
            {
                var index = this.Extensions.FindIndex(x => x.ToLowerInvariant() == ext.ToLowerInvariant());

                if (index == -1)
                    return;

                this.Extensions.Add(ext.ToLowerInvariant());
            }
        }

        /// <summary>
        /// Remover a extensão do asset atual.
        /// </summary>
        /// <param name="ext">Extensão que será removida.</param>
        public void RemoveExtension(string ext)
        {
            lock (this.Extensions)
            {
                var index = this.Extensions.FindIndex(x => x.ToLowerInvariant() == ext.ToLowerInvariant());

                if (index == -1)
                    return;

                this.Extensions.RemoveAt(index);
            }
        }
    }
}
