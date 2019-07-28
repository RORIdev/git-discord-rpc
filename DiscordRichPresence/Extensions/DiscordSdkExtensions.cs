using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSdk
{
    public static class DiscordSdkExtensions
    {
        static void DefaultCallbackHandler(Result r) { /* ignored */ }

        public static void UpdateActivity(this ActivityManager am, Activity activity)
        {
            am.UpdateActivity(activity, DefaultCallbackHandler);
        }

        public static void ClearActivity(this ActivityManager am)
        {
            am.ClearActivity(DefaultCallbackHandler);
        }
    }
}
