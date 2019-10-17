using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiscordRichPresence.Net.Entities;
using DiscordRichPresence.Net.EventArgs;
using Newtonsoft.Json.Linq;

namespace DiscordRichPresence.Net
{
    public delegate Task DiscordCommandCallback(DiscordPipeClient connection, DiscordCommand response);

    public class DiscordPipeClient
    {
        public event Func<Task> Connected;
        public event Func<Exception, Task> Errored;
        public event Func<Task> Disconnected;
        public event Func<ReadyEventArgs, Task> Ready;

        public int RpcVersion { get; internal set; }
        public DiscordUser CurrentUser { get; internal set; }
        public DiscordConfig Environment { get; internal set; }

        private NamedPipeClientStream Pipe;
        private ConcurrentDictionary<string, DiscordCommandCallback> Callbacks;

        internal ulong ApplicationId;

        public DiscordPipeClient(ulong application_id)
        {
            this.Callbacks = new ConcurrentDictionary<string, DiscordCommandCallback>();
            this.ApplicationId = application_id;
        }

        public async Task ConnectAsync(int id = 0)
        {
            if (id < 0 || id > 9)
                throw new ArgumentNullException(nameof(id), "Pipe id must be valid range: 0-9");

            if (this.Pipe != null)
                if (this.Pipe.IsConnected)
                    return;

            this.Pipe = new NamedPipeClientStream(".", $"discord-ipc-{id}", PipeDirection.InOut);

            try
            {
                await this.Pipe.ConnectAsync();
                await Task.Delay(1000);

                if (!this.Pipe.IsConnected)
                    throw new InvalidOperationException("Pipe is not connected.");

                _ = Task.Run(this.InitializeAsync);

                await this.Connected?.Invoke();
            }
            catch (Exception ex)
            {
                await this.Errored?.Invoke(ex);
                await this.CloseAsync();
            }
        }

        public async Task DisconnectAsync()
        {
            await this.CloseAsync();
        }

        async Task CloseAsync()
        {
            if (this.Pipe == null || !this.Pipe.IsConnected)
                return;

            if (this.Pipe != null)
            {
                this.Pipe.Dispose();
                this.Pipe = null;
            }

            await this.Disconnected?.Invoke();
        }

        async Task InitializeAsync()
        {
            await this.SendAsync(DiscordFrameType.Handshake, new DiscordHandshake { ClientId = this.ApplicationId });
            await this.ReadPipeAsync();
        }

        async Task ReadPipeAsync()
        {
            while (this.Pipe != null && this.Pipe.IsConnected)
            {
                try
                {
                    var raw = new byte[this.Pipe.InBufferSize];

                    if (await this.Pipe.ReadAsync(raw, 0, raw.Length) > 0)
                    {
                        var frame = new DiscordFrame(raw);

                        Debug.WriteLine("[DISCORD-IPC] ReadPipeAsync(): <<: {0}", args: frame.GetJson());

                        switch (frame.Type)
                        {
                            case DiscordFrameType.Frame:
                                await this.HandleFrameAsync(frame);
                                break;

                            case DiscordFrameType.Close:
                                await this.CloseAsync();
                                break;
                        }
                    }

                    await Task.Delay(1);
                }
                catch (Exception ex)
                {
                    await this.Errored?.Invoke(ex);
                    await this.CloseAsync();
                }
            }
        }

        internal async Task SendAsync(DiscordFrameType type, object payload)
        {
            if (this.Pipe == null || !this.Pipe.IsConnected)
                return;

            var result = new DiscordFrame()
                .WithType(type)
                .WithPayload(payload)
                .GetBytes();

            await this.Pipe.WriteAsync(result, 0, result.Length);
        }

        internal async Task SendCommandAsync(DiscordFrameType type, DiscordCommand command, DiscordCommandCallback callback = null)
        {
            if (this.Pipe == null || !this.Pipe.IsConnected)
                return;

            var result = new DiscordFrame()
                .WithType(type)
                .WithPayload(command)
                .GetBytes();

            if (callback != null)
                this.Callbacks.AddOrUpdate(command.Nonce, callback, (key, old) => callback);

            await this.Pipe.WriteAsync(result, 0, result.Length);
        }

        internal async Task HandleFrameAsync(DiscordFrame frame)
        {
            var payload = frame.Payload.ToObject<DiscordCommand>();

            switch (payload.Command)
            {
                case DiscordCommandType.Dispatch:
                    await this.HandleEventAsync(frame, payload);
                    break;
            }
        }

        public Task SetActivityAsync(Action<DiscordActivity> activity)
        {
            var model = new DiscordActivity();
            activity(model);
            return this.SetActivityAsync(model, null);
        }

        public async Task SetActivityAsync(DiscordActivity activity, int? pid = null)
        {
            var command = new DiscordCommand()
            {
                Command = DiscordCommandType.SetActivity,
                Arguments = JObject.FromObject(new
                {
                    pid = pid.GetValueOrDefault(Process.GetCurrentProcess().Id),
                    activity
                })
            };

            var result = new DiscordFrame()
                .WithType(DiscordFrameType.Frame)
                .WithPayload(command)
                .GetBytes();

            await this.Pipe.WriteAsync(result, 0, result.Length);
        }

        protected async Task HandleEventAsync(DiscordFrame frame, DiscordCommand command)
        {
            switch (command.Event)
            {
                case DiscordEventType.Ready:
                {
                    var ready = command.Data.ToObject<ReadyEventArgs>();
                    ready.Client = this;

                    this.Environment = ready.Configuration;
                    this.RpcVersion = ready.Version;
                    this.CurrentUser = ready.User;

                    await this.Ready?.Invoke(ready);
                }
                break;
            }
        }
    }
}