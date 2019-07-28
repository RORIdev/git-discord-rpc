using System;
using System.Threading;
using DiscordSdk;

namespace DiscordRichPresence
{
    public class DiscordManager
    {
        private Discord discord;
        private Thread callbacksThread;
        private ActivityManager activityManager;
        private readonly object activityLocker = new object();
        private Activity cache;

        public DiscordManager(long application_id, byte id)
        {
            Environment.SetEnvironmentVariable("DISCORD_INSTANCE_ID",
                id.ToString(),
                EnvironmentVariableTarget.Process);

            discord = new Discord(application_id, (long)CreateFlags.Default);
            discord.SetLogHook(LogLevel.Info, (l, m) => { });

            callbacksThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        discord.RunCallbacks();
                    }
                    catch { /* ignored */ }
                    Thread.Sleep(1000 / 60);
                }
            });

            callbacksThread.IsBackground = true;
            callbacksThread.SetApartmentState(ApartmentState.MTA);

            cache = new Activity();
            cache.Timestamps = new ActivityTimestamps();
            cache.Party = new ActivityParty();
            cache.Secrets = new ActivitySecrets();
            cache.Assets = new ActivityAssets();
        }

        public void Initialize()
        {
            activityManager = discord.GetActivityManager();
            Thread.Sleep(5000);
            callbacksThread.Start();

            SetActivity(new ActivityBuilder() 
                .WithLargeAsset("visualstudio_small", "Visual Studio")
                .WithTimestamps(DateTime.Now));
        }


        public void UpdateActivity(Action<ActivityBuilder> cb)
        {
            var temp = new ActivityBuilder(cache);
            cb(temp);
            SetActivity(temp.Build());
        }

        public void SetActivity(Activity activity)
        {
            lock (activityLocker)
            {
                activityManager.UpdateActivity(cache = activity);
            }
        }

        public void ClearActivity()
        {
            lock (activityLocker)
            {
                activityManager.ClearActivity();
                cache = default;
            }
        }
    }
}
