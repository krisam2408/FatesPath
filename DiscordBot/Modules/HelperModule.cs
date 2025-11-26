using Discord.WebSocket;
using FatesPathLib.Configuration;

namespace DiscordBot.Modules;

internal class HelperModule : BaseModule
{
    public string[] Ping(SocketUserMessage message, FateConfig context) => ["FateCast Bot is available"];
}
