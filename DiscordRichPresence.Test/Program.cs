using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRichPresence.Net;

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
            };

            await client.ConnectAsync(0);

            while(true)
            {
                await Task.Delay(100);
            }
        }
    }
}
