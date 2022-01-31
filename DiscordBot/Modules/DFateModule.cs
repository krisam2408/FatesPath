using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services.ConfigurationLib;
using FatesPathLib;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class DFateModule : ModuleBase<SocketCommandContext>
    {
        [Command("dfate")]
        [Summary("Special Fate roll")]
        public async Task DFateRollAsync()
        {
            FateCaster caster = new(Config.Instance.FateConfig);
            SocketUser currentUser = Context.User;

            PathPool pool = new(DiceType.D6, 4);
            ResultPath result = caster.CastFate(pool);

            int rollResult = 0;

            foreach (Dice d in result.Results)
            {
                switch (d.Result)
                {
                    case 3:
                    case 4:
                        rollResult--;
                        break;
                    case 5:
                    case 6:
                        rollResult++;
                        break;
                }
            }

            string strResult = string.Format("{0}{1}", rollResult > 0 ? "+" : "", rollResult);

            string reply = $"{currentUser.Username} rolled {strResult}. ({result.ResultsString})";

            await ReplyAsync(reply);
        }

        [Command("dfate")]
        [Summary("Special Fate roll")]
        public async Task DFateRollPlusAsync(string args)
        {
            FateCaster caster = new(Config.Instance.FateConfig);
            SocketUser currentUser = Context.User;

            PathPool pool = new(DiceType.D6, 4);
            ResultPath result = caster.CastFate(pool);

            int rollResult = 0;

            foreach (Dice d in result.Results)
            {
                switch (d.Result)
                {
                    case 3:
                    case 4:
                        rollResult--;
                        break;
                    case 5:
                    case 6:
                        rollResult++;
                        break;
                }
            }

            try
            {
                int bonus = int.Parse(args);

                rollResult += bonus;

                string strResult = string.Format("{0}{1}", rollResult > 0 ? "+" : "", rollResult);

                string reply = $"{currentUser.Username} rolled {strResult}. ({result.ResultsString})";

                await ReplyAsync(reply);
            }
            catch (Exception)
            {
                await ReplyAsync("Invalid command format");
            }

        }
    }
}
