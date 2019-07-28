using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSdk
{
    public class ActivityBuilder
    {
        private Activity activity;

        public ActivityBuilder()
        {
            activity = new Activity();
            activity.Assets = new ActivityAssets();
            activity.Party = new ActivityParty();
            activity.Secrets = new ActivitySecrets();
            activity.Timestamps = new ActivityTimestamps();
        }

        public ActivityBuilder(Activity other)
        {
            activity = other;
        }

        public ActivityBuilder WithDetails(string details = null)
        {
            activity.Details = details;
            return this;
        }

        public ActivityBuilder WithState(string state = null)
        {
            activity.State = state;
            return this;
        }

        public ActivityBuilder WithTimestamps(DateTime? start = null, DateTime? end = null)
        {
            if (start.HasValue)
                activity.Timestamps.Start = (long)(start.Value.ToUniversalTime()).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            else
                activity.Timestamps.Start = 0;

            if (end.HasValue)
                activity.Timestamps.End = (long)(end.Value.ToUniversalTime()).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            else
                activity.Timestamps.End = 0;

            return this;
        }

        public ActivityBuilder WithTimestamps(long start = 0, long end = 0)
        {
            activity.Timestamps.Start = start;
            activity.Timestamps.End = end;
            return this;
        }

        public ActivityBuilder WithLargeAsset(string image = null, string text = null)
        {
            activity.Assets.LargeImage = image;
            activity.Assets.LargeText = text;
            return this;
        }

        public ActivityBuilder WithSmallAsset(string image= null, string text = null)
        {
            activity.Assets.SmallImage= image;
            activity.Assets.SmallText = text;
            return this;
        }

        public ActivityBuilder WithType(ActivityType type = ActivityType.Playing)
        {
            activity.Type = type;
            return this;
        }

        public Activity Build()
        {
            return activity;
        }

        public static implicit operator Activity (ActivityBuilder ab) =>
            ab.Build();
    }
}
