using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRichPresence.Net;
using DiscordRichPresence.Net.Entities;

namespace DiscordRichPresence.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new DiscordPipeClient(421688819868237824);

            client.Connected += () =>
            {
                Console.WriteLine("Connected.");
                return Task.CompletedTask;
            };

            client.Disconnected += () =>
            {
                Console.WriteLine("Disconnected.");
                return Task.CompletedTask;
            };

            client.Errored += async ex =>
            {
                Console.WriteLine(ex);
            };

            client.Ready += async e =>
            {
                Console.WriteLine("Connected as {0}#{1}", e.User.Username, e.User.Discriminator);

                await client.SetActivityAsync(x =>
                {
                    x.State = "Working On: MyProject.cs";
                    x.Details = "Editing: MyFile.cs";
                    x.Assets = new DiscordActivityAssets
                    {
                        LargeImage = "file_type_csharp",
                        LargeText = "C# Source File",
                        SmallImage = "visualstudio_small",
                        SmallText = "Solution: MySolution"
                    };

                    x.Timestamps = new DiscordActivityTimestamps
                    {
                        StartTime = DateTimeOffset.Now,
                        EndTime = DateTimeOffset.Now.AddMinutes(15)
                    };

                    x.Secrets = new DiscordActivitySecrets
                    {
                        Join = Guid.NewGuid().ToString("D"),
                        Match = Guid.NewGuid().ToString("D"),
                        Spectate = Guid.NewGuid().ToString("D")
                    };
                });
            };

            await client.ConnectAsync(0);

            while(true)
            {
                await Task.Delay(100);
            }
        }
    }
}
