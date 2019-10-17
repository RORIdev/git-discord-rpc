﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class DiscordConfiguration
    {
        [JsonIgnore]
        public static readonly DiscordConfiguration Empty = new DiscordConfiguration();

        /// <summary>
        /// Id da aplicação do discord.
        /// </summary>
        [JsonProperty("application_id")]
        public ulong ApplicationId { get; set; } = 421688819868237824UL;

        /// <summary>
        /// Determina o número da instância que o ipc do discord está. Valores válidos entre 0 e 9
        /// </summary>
        [JsonProperty("current_instance_id")]
        public int CurrentInstanceId { get; set; } = 0;

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