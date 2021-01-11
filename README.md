# gitRPC for VS
Displays what file you're working in visual studio in discord and the current git branch. - [VS Marketplace](http://marketplace.visualstudio.com/items?itemName=gitrpc.gitrpc)

# Example

![Example Discord Precense](https://i.imgur.com/DxJTWOW.gif)

# Features
- Display current file that you are editing.
- ***Display current git `branch`***
- ***Interop with StormDevelopment/DiscordRichPresense***
- Display elapsed time since visual studio is open.
- Capability to enable/disable time.
- Capability to enable/disable project name that you are working.
- Capability to enable/disable solution name that you are working.
- Capability to enable/disable file name that you are editing.
- Capability to enable/disable time reset when file opened/closed.
- Capability to use custom discord application id.
- Capability to use custom assets and texts.
- Capability to use different clients (if you have more that one discord running)
- Full .NET discord rich presence implementation (See above)

# Configuration
Located in `%USERPROFILE%\Storm Development Software\Discord Rich Presence`
By default configuration have standard values for an discord application with desired assets. But you can customize everything.

- discord.json: Discord rich presence related settings and capabilities settings.
- assets.json: Assets related settings, list of assets with list of file extensions supported by asset.
- localization.json: Localization related settings, list of localized string (per user localizable). For assets localization put at (@) in start of `"text": "@my_localizaiton_key"`
<br/>

# Contributing
- If you have an missing file type, you can submit an request with an SVG (scale vector) or full 512x512 high quality image to upload to our rich presence assets (or use a custom application too).
- If you have an code contribution, please follow .editorconfig rules in your IDE, and fell free to help us to develop this extension.

# Notes
Thanks for [DSharpPlus/DotNetRPC](https://github.com/DSharpPlus/DotnetRPC/) for provide .NET DiscordRpc implementation, i extracted code part from that and used in this extension.

Thanks for [StormDevelopment/DiscordRichPresense](https://github.com/StormDevelopment/DiscordRichPresense) for serving as a base for this extension,