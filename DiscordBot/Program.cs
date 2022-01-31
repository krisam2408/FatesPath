using Discord;
using Discord.WebSocket;
using DiscordBot.Services.ConfigurationLib;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            _client = new();
            _client.Log += Log;

            string token = Config.Instance.Token;

            _commandHandler = new(_client, new());
            await _commandHandler.InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage log)
        {
            switch (log.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"[General/{log.Severity}] {log}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
