using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services.ConfigurationLib;
using FatesPathLib;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("roll")]
        [Summary("Roll dices")]
        public async Task RollFateAsync(string args)
        {
            FateCaster caster = new(Config.Instance.FateConfig);
            SocketUser currentUser = Context.User;

            string[] cmdArray = args.ToLower().Split(',');

            foreach (string cmd in cmdArray)
            {
                try
                {
                    int[] _params = cmd.Trim()
                        .Split('d')
                        .Select(i => int.Parse(i))
                        .ToArray();

                    DiceType dice = (DiceType)_params[1];

                    PathPool pool = new(dice, _params[0]);
                    ResultPath result = caster.CastFate(pool);

                    string reply = $"{currentUser.Username} rolled {cmd.ToUpper()}: {result.ResultsString}";

                    await ReplyAsync(reply);

                }
                catch (Exception)
                {
                    await ReplyAsync("Invalid command format");
                }
            }
        }

        [Command("coin")]
        [Summary("Flip a Coin")]
        public async Task CoinFateAsync()
        {
            FateCaster caster = new(Config.Instance.FateConfig);
            SocketUser currentUser = Context.User;

            DiceType dice = DiceType.Coin;

            PathPool pool = new(dice, 1);
            ResultPath result = caster.CastFate(pool);

            int diceResult = result.Results[0].Result;

            string reply = diceResult == 1 ? $"{currentUser.Username} flipped NO" : $"{currentUser} flipped YES";

            await ReplyAsync(reply);
        }

    }
}
