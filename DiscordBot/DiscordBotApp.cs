using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Configuration;
using DiscordBot.Modules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot;

public sealed class DiscordBotApp
{
    private readonly DiscordSocketClient m_client;
    private readonly string m_discordToken;

    private DiscordBotApp(AppConfig appConfig) 
    {
        DiscordSocketConfig config = new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        m_client = new(config);
        m_discordToken = appConfig.DiscordToken;
    }

    public static DiscordBotApp Build(AppConfig config)
    {
        DiscordBotApp app = new(config);

        return app;
    }

    public async Task Run()
    {
        m_client.Log += HandleLogs;
        m_client.Ready += Ready;
        m_client.SlashCommandExecuted += SlashCommandListener;

        await m_client.LoginAsync(TokenType.Bot, m_discordToken);
        await m_client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task SlashCommandListener(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "ping":
                await HelperModule.Ping(command);
                break;
            case "moneda":
                await GeneralModule.Coin(command);
                break;
            case "dados":
                await GeneralModule.Roll(command);
                break;
            case "cod":
                await CoDModule.Cast(command);
                break;
            default:
                await command.RespondAsync("Comando no encontrado");
                break;
        }
    }

    private async Task Ready()
    {
        List<ApplicationCommandProperties> appCommands = new();
        try
        {

            SlashCommandBuilder pingCommand = new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("Revisa que el bot esté activo");
            appCommands.Add(pingCommand.Build());

            SlashCommandBuilder coinCommand = new SlashCommandBuilder()
                .WithName("moneda")
                .WithDescription("Lanza una moneda");
            appCommands.Add(coinCommand.Build());

            SlashCommandBuilder rollCommand = new SlashCommandBuilder()
                .WithName("dados")
                .WithDescription("Lanza dados")
                .AddOption("cantidad", ApplicationCommandOptionType.Integer, "Cantidad de dados a lanzar", isRequired: true)
                .AddOption("caras", ApplicationCommandOptionType.Integer, "Caras de los dados", isRequired: true);
            appCommands.Add(rollCommand.Build());

            SlashCommandBuilder castCommand = new SlashCommandBuilder()
                .WithName("cod")
                .WithDescription("Lanza dados con reglas de CoD")
                .AddOption("cantidad", ApplicationCommandOptionType.Integer, "Cantidad de dados a lanzar", isRequired: true)
                .AddOption("argumentos", ApplicationCommandOptionType.String, "argumentos opcionales", isRequired: false);
            appCommands.Add(castCommand.Build());

            await m_client.BulkOverwriteGlobalApplicationCommandsAsync(appCommands.ToArray());
        }
        catch(HttpException ex)
        {
            string log = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);
            Console.WriteLine(log);
        }
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
}
