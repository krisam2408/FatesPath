using Discord.WebSocket;
using System;

namespace DiscordBot.Modules;

internal abstract class BaseModule
{
    internal string[] GetArguments(SocketUserMessage message)
    {
        string content = message.Content[1..];
        string[] teils = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return teils[1..];
    }
}
