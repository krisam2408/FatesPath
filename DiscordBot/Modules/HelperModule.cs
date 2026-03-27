using Discord.WebSocket;

namespace DiscordBot.Modules;

internal class HelperModule : BaseModule
{
    public string[] Ping(SocketUserMessage message) => ["FateCast Bot está disponible"];
}
