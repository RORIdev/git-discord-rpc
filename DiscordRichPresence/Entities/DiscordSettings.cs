using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class DiscordSettings
    {
        [JsonIgnore]
        public static readonly DiscordSettings Empty = new DiscordSettings();

        /// <summary>
        /// Id da aplicação do discord.
        /// </summary>
        [JsonProperty("application_id")]
        public ulong ApplicationId { get; set; } = 421688819868237824UL;

        /// <summary>
        /// Determina o número da instância que o ipc do discord está. Valores válidos entre 0 e 9
        /// </summary>
        [JsonProperty("discord_instance_id")]
        public int PipeInstanceId { get; set; } = 0;

        /// <summary>
        /// Determina se irá mostrar o nome do projeto ao invés da solução. Padrão: Não.
        /// </summary>
        [JsonProperty("display_project_name")]
        public bool DisplayProject { get; set; } = false;

        /// <summary>
        /// Determina se irá mostrar o nome da solução. Padrão: Sim
        /// </summary>
        [JsonProperty("display_solution_name")]
        public bool DisplaySolution { get; set; } = true;

        /// <summary>
        /// Determina se irá reiniciar o tempo de exibição a cada vez que alterar de arquivo.
        /// </summary>
        [JsonProperty("auto_reset_timestamp")]
        public bool AutoResetTimestamp { get; set; } = false;

        /// <summary>
        /// Determina se irá mostrar o tempo decorrido. Padrão: Sim
        /// </summary>
        [JsonProperty("display_timestamp")]
        public bool DisplayTimestamp { get; set; } = true;
    }
}
