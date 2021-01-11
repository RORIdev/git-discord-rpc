using System.IO;
using System.Linq;

namespace DiscordRichPresence
{
    public static class Utilities
    {
        public static DirectoryInfo GetGitDirectory(string CurrentPath) {
            var di = new DirectoryInfo(CurrentPath);
            if (di.GetDirectories().Any(x => x.Name == ".git")) return di;
            if (di.Parent == null) return null;
            else return GetGitDirectory(di.Parent.FullName);
        }
    }
}
