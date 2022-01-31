using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class HelperModule : ModuleBase<SocketCommandContext>
    {
        private readonly string[] messages = new string[]
        {
            "FateCastBot commands",
            "----------",
            "Helper Module",
            "!ping                     -> Checks for bot availability (Example: !ping)",
            "!help                     -> Shows bot commands and options (Example: !help)",
            "option {cmd}              -> Shows options for specific command {cmd} (Example: !help cast)",
            "----------",
            "General Module",
            "!roll {n1}[Dd]{n2}        -> Roll 'n1' dices of 'n2' faces (Example: !roll 3d10)",
            "!roll {n1}[Dd]{n2},...    -> Roll 'n1' dices of 'n2' faces, followed by another roll in the same format (Example: !roll 1d4,2D6,3d10)",
            "!coin                     -> Flips a coin with a 'YES' or 'NO' result (Example: !coin)",
            "----------",
            "Chronicles of Darkness Module",
            "!cast {n}options          -> Makes a roll with Chronicles of Darkness rules. It also has additional options (Example: !cast 5)",
            "option [Tt][Aa]{n}        -> Sets the number of the throw again rule (Example: !cast 5ta9)",
            "option [Dd][Ii]{n}        -> Changes the default dice to another one for one roll (Example: !cast 3di4)",
            "option [Dd][Ff]{n}        -> Changes the default throw difficulty for one roll (Example: !cast 7df5)",
            "option -[Ii]              -> Implements the 'Inspired' condition, setting 3 as the minimum success value for an exceptional success throw (Example: !cast 5-i)",
            "option -[Rr]              -> Sets the quality 'rote' for the throw (Example: !cast 22-r)",
            "option 0                  -> Implements the chance die rules (Example: !cast 0)",
            "* It is possible to sum the options from !cast command in an additive way (Example: !cast 6ta9-r-i)",
            "----------",
            "Fate Accelerated Module",
            "!dfate                    -> Makes a roll with Fate Accelerated rules. It also has additional rules (Example: !dfate)",
            "option [+-]{n}            -> Adds a bonus to the roll (Example: !dfate -2)"
        };

        [Command("ping")]
        [Summary("Checks bot availability")]
        public async Task PingAsync()
        {
            await ReplyAsync("FateCast Bot is available");
        }

        [Command("help")]
        [Summary("Shows the available commands")]
        public async Task HelpAsync()
        {
            foreach(string msg in messages)
                await ReplyAsync(msg);
        }

        [Command("help")]
        [Summary("Shows options for specific commands")]
        public async Task HelpAsync(string cmd)
        {
            string cmdE = cmd.ToLower().Replace("!", string.Empty);
            await ReplyAsync($"Help options for command !{cmdE}");
            switch(cmdE)
            {
                case "ping":
                    await ReplyAsync(messages[3]);
                    break;
                case "help":
                    for( int i = 4; i <= 5; i++)
                        await ReplyAsync(messages[i]);
                    break;
                case "roll":
                    for (int i = 8; i <= 9; i++)
                        await ReplyAsync(messages[i]);
                    break;
                case "coin":
                    await ReplyAsync(messages[10]);
                    break;
                case "cast":
                    for (int i = 13; i <= 20; i++)
                        await ReplyAsync(messages[i]);
                    break;
                case "dfate":
                    for (int i = 23; i <= 24; i++)
                        await ReplyAsync(messages[i]);
                    break;
                default:
                    await ReplyAsync("Command not recognized");
                    break;
            }
        }
    }
}
