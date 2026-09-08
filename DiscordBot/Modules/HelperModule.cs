using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordBot.Modules;

internal static class HelperModule
{
    public static async Task Ping(SocketSlashCommand cmd)
    {
        await cmd.RespondAsync("Hola, aquí estoy!");
    } 
}
