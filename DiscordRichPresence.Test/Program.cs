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
            var pipe = new DiscordPipeConnection(421688819868237824);

            pipe.Connected += async () =>
            {
                Console.WriteLine("Connected.");
            };

            pipe.Disconnected += async () =>
            {
                Console.WriteLine("Disconnected.");
            };

            pipe.Errored += async ex =>
            {
                Console.WriteLine(ex);
            };

            await pipe.ConnectAsync(0);

            while(true)
            {
                await Task.Delay(100);
            }
        }
    }
}
