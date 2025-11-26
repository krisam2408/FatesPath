using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Configuration;
using DiscordBot.Modules;
using System;
using System.Threading.Tasks;

namespace DiscordBot;

public sealed class DiscordBotApp
{
    private readonly DiscordSocketClient m_client;
    private readonly CommandHandler m_commands;
    private readonly string m_discordToken;

    private DiscordBotApp(AppConfig appConfig) 
    {
        DiscordSocketConfig config = new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        m_client = new(config);
        m_commands = new(appConfig.FateConfig);
        m_discordToken = appConfig.DiscordToken;
    }

    public static DiscordBotApp Build(AppConfig config)
    {
        DiscordBotApp app = new(config);

        return app;
    }

    public async Task Run()
    {
        await m_commands.InstallModule<HelperModule>();
        await m_commands.InstallModule<GeneralModule>();
        await m_commands.InstallModule<CoDModule>();

        m_client.Log += HandleLogs;
        m_client.MessageReceived += HandleMessages;

        await m_client.LoginAsync(TokenType.Bot, m_discordToken);
        await m_client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task HandleLogs(LogMessage log)
    {
        Console.ForegroundColor = log.Severity switch
        {
            LogSeverity.Critical or LogSeverity.Error => ConsoleColor.Red,
            LogSeverity.Warning => ConsoleColor.Yellow,
            _ => ConsoleColor.White
        };

        Console.WriteLine($"[General/{log.Severity}] {log}");
        Console.ResetColor();
    }

    private async Task HandleMessages(SocketMessage message)
    {
        SocketUserMessage userMessage = (SocketUserMessage)message;
        if (userMessage == null)
            return;

        int argPos = -1;

        bool isBot = userMessage.Author.IsBot;
        bool hasPrefix = userMessage.HasCharPrefix('!', ref argPos);
        bool cmdAtStart = argPos == 1;

        if (isBot || !cmdAtStart)
            return;

        await m_commands.ExecuteCommand(userMessage);
    }
}
