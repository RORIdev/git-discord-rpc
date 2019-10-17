namespace DiscordRichPresence.Net.Entities
{
    public enum DiscordUserType
    {
        None = 0,
        DiscordEmployee = 1 << 0,
        DiscordPartner = 1 << 1,
        HypeSquadEvents = 1 << 2,
        BugHunter = 1 << 3,
        HouseBravery = 1 << 6,
        HouseBrilliance = 1 << 7,
        HouseBalance = 1 << 8,
        EarlySupporter = 1 << 9,
        TeamUser = 1 << 10
    }
}
