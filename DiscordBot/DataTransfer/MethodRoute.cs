using Discord.WebSocket;
using FatesPathLib.Configuration;
using System;
using System.Reflection;

namespace DiscordBot.DataTransfer;

internal sealed class MethodRoute
{
    public Type Type { get; set; }
    public MethodInfo Function { get; set; }

    public string[] ExecuteMethod(SocketUserMessage message, FateConfig config)
    {
        ConstructorInfo constructor = Type.GetConstructor([]);
        object instance = constructor.Invoke(null);

        string[] result = (string[])Function.Invoke(instance, [message, config]);
        return result;
    }
}
