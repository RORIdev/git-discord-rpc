using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordRichPresence.Entities.Configuration
{
    public partial class AssetsConfig
    {
        public static readonly AssetsConfig Default = new AssetsConfig
        {
            Extensions = new List<DiscordAsset>
            {
                DiscordAsset.Empty,

                new DiscordAsset("file_type_asp", "ASP/ASP.NET Server Pages", "asp"),
                new DiscordAsset("file_type_aspx","ASP/ASP.NET Server Pages", "aspx"),
                new DiscordAsset("file_type_bat", "Windows/Unix Command Script File", "bat", "cmd", "com", "sh"),
                new DiscordAsset("file_type_cpp", "C/C++ Source File", "cpp", "c", "cc", "c", "cxx", "ino"),
                new DiscordAsset("file_type_cppheader", "C/C++ Header File", "h", "hpp", "hh", "hxx"),
                new DiscordAsset("file_type_csharp", "C# Source File", "cs", "cake"),
                new DiscordAsset("file_type_csproj", "C# Project File", "csproj"),
                new DiscordAsset("file_type_fsharp2", "F# Source File", "fs", "fsx", "fsi", "fsscript"),
                new DiscordAsset("file_type_html", "HTML File", "html", "htm", "hta"),
                new DiscordAsset("file_type_js", "Javascript File", "js", "jsx", "jsm"),
                new DiscordAsset("file_type_json_official", "JSON File", "json"),
                new DiscordAsset("file_type_light_config", "Settings File", "settings", "cfg", "inf", "ini", "properties"),
                new DiscordAsset("file_type_njsproj", "NodeJS Project", "njsproj"),
                new DiscordAsset("file_type_php3", "PHP Source File", "php", "php3", "php4", "php5", "phps", "phpt", "phtml"),
                new DiscordAsset("file_type_python", "Python File", "py", "py3", "pyw"),
                new DiscordAsset("file_type_sln", "Visual Studio Solution File", "sln"),
                new DiscordAsset("file_type_sql", "SQL File", "sql"),
                new DiscordAsset("file_type_text", "Text File", "txt", "log"),
                new DiscordAsset("file_type_typescript", "TypeScript File", "ts", "tsx"),
                new DiscordAsset("file_type_vb", "VB Source File", "vb"),
                new DiscordAsset("file_type_vb", "VB Script File", "vbs"),
                new DiscordAsset("file_type_vbproj", "VB Project File", "vbproj"),
                new DiscordAsset("file_type_vcxproj", "C/C++ Project File", "vcxproj", "vcproj"),
                new DiscordAsset("file_type_xml", "XML File", "xml", "xsl", "xstl","xsd"),
                new DiscordAsset("file_type_yaml", "YAML File", "yaml", "yml")
            }
        };
    }
}
