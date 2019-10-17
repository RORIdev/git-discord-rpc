using System.Runtime.Serialization;

namespace DiscordRichPresence.Pipe
{
    public enum DiscordEventType
    {
        [EnumMember(Value = "READY")]
        Ready,

        [EnumMember(Value = "ERROR")]
        Error,

        [EnumMember(Value = "GUILD_STATUS")]
        GuildStatus,

        [EnumMember(Value = "GUILD_CREATE")]
        GuildCreate,

        [EnumMember(Value = "CHANNEL_CREATE")]
        ChannelCreate,

        [EnumMember(Value = "VOICE_CHANNEL_SELECT")]
        VoiceChannelSelect,

        [EnumMember(Value = "VOICE_STATE_CREATE")]
        VoiceStateCreate,

        [EnumMember(Value = "VOICE_STATE_UPDATE")]
        VoiceStateUpdate,

        [EnumMember(Value = "VOICE_STATE_DELETE")]
        VoiceStateDelete,

        [EnumMember(Value = "VOICE_SETTINGS_UPDATE")]
        VoiceSettingsUpdate,

        [EnumMember(Value = "VOICE_CONNECTION_STATUS")]
        VoiceConnectionStatus,

        [EnumMember(Value = "SPEAKING_START")]
        SpeakingStart,

        [EnumMember(Value = "SPEAKING_STOP")]
        SpeakingStop,

        [EnumMember(Value = "MESSAGE_CREATE")]
        MessageCreate,

        [EnumMember(Value = "MESSAGE_UPDATE")]
        MessageUpdate,

        [EnumMember(Value = "MESSAGE_DELETE")]
        MessageDelete,

        [EnumMember(Value = "NOTIFICATION_CREATE")]
        NotificationCreate,

        [EnumMember(Value = "CAPTURE_SHORTCUT_CHANGE")]
        CaptureShortcutChange,

        [EnumMember(Value = "ACTIVITY_JOIN")]
        ActivityJoin,

        [EnumMember(Value = "ACTIVITY_SPECTATE")]
        ActivitySpectate,

        [EnumMember(Value = "ACTIVITY_JOIN_REQUEST")]
        ActivityJoinRequest,
    }
}
