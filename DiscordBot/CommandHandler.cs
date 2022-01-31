using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Modules;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commands = commands;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModuleAsync<GeneralModule>(null);
            await _commands.AddModuleAsync<CoDModule>(null);
            await _commands.AddModuleAsync<DFateModule>(null);
            await _commands.AddModuleAsync<HelperModule>(null);
        }

        private async Task HandleCommandAsync(SocketMessage msgParam)
        {
            SocketUserMessage msg = (SocketUserMessage)msgParam;
            if (msg == null)
                return;

            int argPos = 0;

            if (!(msg.HasCharPrefix('!', ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos)) || msg.Author.IsBot)
                return;


            SocketCommandContext context = new(_client, msg);

            await _commands.ExecuteAsync(context, argPos, null);
        }
    }
}
