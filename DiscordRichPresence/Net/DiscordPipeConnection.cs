using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordRichPresence.Net
{
    public delegate Task DiscordCommandCallback(DiscordPipeConnection connection, DiscordCommand response);

    public class DiscordPipeConnection
    {
        public event Func<Task> Connected;
        public event Func<Exception, Task> Errored;
        public event Func<Task> Disconnected;

        private NamedPipeClientStream Pipe;
        private CancellationTokenSource Cts;
        private ConcurrentDictionary<string, DiscordCommandCallback> Callbacks;

        private volatile bool IsConnected;

        internal ulong ApplicationId;

        public DiscordPipeConnection(ulong application_id)
        {
            this.ApplicationId = application_id;
        }

        public async Task ConnectAsync(int id)
        {
            if (this.IsConnected)
                return;

            if (id < 0 || id > 9)
                throw new ArgumentNullException(nameof(id), "Pipe id must be valid range: 0-9");

            this.Pipe = new NamedPipeClientStream(".", $"discord-ipc-{id}");

            try
            {
                await this.Pipe.ConnectAsync();
                await Task.Delay(2500);

                if (!this.Pipe.IsConnected)
                    throw new InvalidOperationException("Pipe is not connected.");

                this.Cts = new CancellationTokenSource();
                this.Callbacks = new ConcurrentDictionary<string, DiscordCommandCallback>();

                await this.Connected?.Invoke();
                this.IsConnected = true;

                _ = Task.Run(ReadPipeAsync, this.Cts.Token);
            }
            catch (Exception ex)
            {
                this.Errored?.Invoke(ex);
            }
        }

        public async Task DisconnectAsync()
        {
            if (!this.IsConnected)
                return;

            this.Cts.Cancel();
            this.IsConnected = false;

            if (this.Pipe != null)
            {
                this.Pipe.Dispose();
                this.Pipe = null;
            }

            await this.Disconnected?.Invoke();
        }

        async Task ReadPipeAsync()
        {
            while (!this.Cts.IsCancellationRequested)
            {
                var rawFrame = new byte[this.Pipe.InBufferSize];

                int length;
                if ((length = await this.Pipe.ReadAsync(rawFrame, 0, rawFrame.Length)) > 0)
                {
                    var frame = new DiscordFrame(rawFrame);
                    var payload = frame.Payload;
                    var command = payload.ToObject<DiscordCommand>();
                }
            }
        }

        internal async Task SendCommandAsync(DiscordFrameType type, DiscordCommand command, DiscordCommandCallback callback = null)
        {
            if (!this.IsConnected || !(bool)this.Pipe?.IsConnected)
                return;

            var frame = new DiscordFrame()
                .WithType(type)
                .WithPayload(command);

            if(callback != null)
                this.Callbacks.AddOrUpdate(command.Nonce, callback, (key, old) => callback);

            var result = frame.GetBytes();
            await this.Pipe.WriteAsync(result, 0, result.Length);
        }
    }
}