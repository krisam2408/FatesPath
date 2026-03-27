using Discord.WebSocket;
using DiscordBot.DataTransfer;
using DiscordBot.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot;

public class CommandHandler
{
    private readonly Dictionary<string, MethodRoute> m_methods = new();

    private readonly string[] m_baseMethods = [
        "gettype",
        "tostring",
        "equals",
        "gethashcode",
        "getarguments"
    ];

    public CommandHandler() { }

    internal async Task InstallModule<T>() where T : BaseModule
    {
        Type moduleType = typeof(T);

        MethodInfo[] methods = moduleType
            .GetMethods();

        foreach (MethodInfo method in methods)
        {
            string methodName = method
                .Name
                .ToLower();

            if (m_baseMethods.Contains(methodName))
                continue;

            MethodRoute route = new()
            {
                Type = moduleType,
                Function = method
            };

            m_methods.Add(methodName, route);
        }
    }

    public async Task ExecuteCommand(SocketUserMessage message)
    {
        string content = message.Content[1..];
        string[] teils = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (!m_methods.TryGetValue(teils[0], out MethodRoute method))
            return;

        string[] reply = method.ExecuteMethod(message);

        foreach(string line in reply)
            await message.Channel.SendMessageAsync(line);
    }
}
