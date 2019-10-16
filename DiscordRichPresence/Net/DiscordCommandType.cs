using System.Runtime.Serialization;

namespace DiscordRichPresence.Net
{
    public enum DiscordCommandType
    {
        [EnumMember(Value = "SET_ACTIVITY")]
        SetActivity,

        [EnumMember(Value = "DISPATCH")]
        Dispatch,

        [EnumMember(Value = "AUTHORIZE")]
        Authorize,

        [EnumMember(Value = "AUTHENTICATE")]
        Authenticate,

        [EnumMember(Value = "GET_GUILD")]
        GetGuild,

        [EnumMember(Value = "GET_GUILDS")]
        GetGuilds,

        [EnumMember(Value = "GET_CHANNEL")]
        GetChannel,

        [EnumMember(Value = "GET_CHANNELS")]
        GetChannels,

        [EnumMember(Value = "SUBSCRIBE")]
        Subscribe,

        [EnumMember(Value = "UNSUBSCRIBE")]
        Unsubscribe,

        [EnumMember(Value = "SET_USER_VOICE_SETTINGS")]
        SetUserVoiceSettings,

        [EnumMember(Value = "SELECT_VOICE_CHANNEL")]
        SelectVoiceChannel,

        [EnumMember(Value = "GET_SELECTED_VOICE_CHANNEL")]
        GetSelectedVoiceChannel,

        [EnumMember(Value = "SELECT_TEXT_CHANNEL")]
        SelectTextChannel,

        [EnumMember(Value = "GET_VOICE_SETTINGS")]
        GetVoiceSettings,

        [EnumMember(Value = "SET_VOICE_SETTINGS")]
        SetVoiceSettings,

        [EnumMember(Value = "CAPTURE_SHORTCUT")]
        CaptureShortcut,

        [EnumMember(Value = "SET_CERTIFIED_DEVICES")]
        SetCertifiedDevices,

        [EnumMember(Value = "SEND_ACTIVITY_JOIN_INVITE")]
        SendActivityJoinInvite,

        [EnumMember(Value = "CLOSE_ACTIVITY_REQUEST")]
        CloseActivityRequest,

    }
}
