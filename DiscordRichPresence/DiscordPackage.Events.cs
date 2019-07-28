using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRichPresence.Entities.Configuration;
using DiscordSdk;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace DiscordRichPresence
{
    public sealed partial class DiscordPackage
    {
        private DTE _dte;
        private Events _events;

        public T GetService<T>() =>
            (T)GetService(typeof(T));

        private void NotifyWindowActivated(Window activated, Window last)
        {
            if(activated.Document == null)
            {
                discord.UpdateActivity(x =>
                {
                    x.WithState(string.Empty);
                    x.WithDetails(string.Empty);
                    x.WithLargeAsset("visualstudio_small", "Visual Studio");
                    x.WithSmallAsset();

                    if (config.Discord.AutoResetTimestamp && config.Discord.DisplayFileName)
                        x.WithTimestamps(GetDiscordTimestamp());
                });
            }
            else
            {
                if(TryParseDocument(activated, out var file, out var file_ext, out var project, out var project_ext))
                {
                    discord.UpdateActivity(x =>
                    {
                        if (!string.IsNullOrEmpty(file))
                            x.WithDetails($"Editing: {file}");

                        if (!string.IsNullOrEmpty(project))
                        {
                            x.WithState($"Working on: {project}");
                        }

                        if (!string.IsNullOrEmpty(file))
                        {
                            var asset = config.Assets[file_ext];
                            x.WithLargeAsset(asset.Key, asset.Value);
                        }

                        if (config.Discord.AutoResetTimestamp && config.Discord.DisplayFileName)
                            x.WithTimestamps(GetDiscordTimestamp());
                    });
                }
                else
                {
                    discord.UpdateActivity(x =>
                    {
                        x.WithLargeAsset("visualstudio_small", "Visual Studio");

                        if (config.Discord.AutoResetTimestamp && config.Discord.DisplayFileName)
                            x.WithTimestamps(GetDiscordTimestamp());
                    });
                }
            }
        }

        private long _timestamp = default;

        private long GetDiscordTimestamp()
        {
            if (config.Discord.DisplayTimestamp)
            {
                if (config.Discord.AutoResetTimestamp)
                {
                    return _timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                }
                else
                {
                    return _timestamp;
                }
            }
            else
            {
                return default;
            }
        }

        private bool TryParseDocument(Window w, out string file_name, out string file_ext, out string project_name, out string project_ext)
        {
            file_name = null;
            file_ext = null;
            project_name = null;
            project_ext = null;

            if (w == null)
                return false;

            var d = w.Document;

            if(w.Project != null && !string.IsNullOrEmpty(w.Project.FileName))
            {
                if (!File.Exists(w.Project.FileName))
                {
                    project_name = Path.GetFileNameWithoutExtension(w.Project.FileName);
                    project_ext = Path.GetExtension(w.Project.FileName).Substring(1);
                }
                else
                {
                    var info = new FileInfo(w.Project.FileName);
                    project_name = Path.GetFileNameWithoutExtension(info.FullName);
                    project_ext = Path.GetExtension(info.FullName).Substring(1);
                }
            }
            else
            {
                project_name = string.Empty;
                project_ext = string.Empty;
            }

            if (d != null && config.Discord.DisplayFileName)
            {

                var path = d.FullName;

                if (!File.Exists(path))
                {
                    file_name = Path.GetFileName(path);
                    file_ext = Path.GetExtension(path).Substring(1);
                }
                else
                {
                    var info = new FileInfo(d.FullName);
                    file_name = Path.GetFileName(info.FullName);
                    file_ext = Path.GetExtension(info.Extension).Substring(1);
                }
            }
            else
            {
                file_name = string.Empty;
                file_ext = string.Empty;
            }

            return true;
        }
    }
}