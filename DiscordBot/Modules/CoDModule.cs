using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Extensions;
using DiscordBot.Services.ConfigurationLib;
using FatesPathLib;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class CoDModule : ModuleBase<SocketCommandContext>
    {
        [Command("cast")]
        [Summary("Cast a dice")]
        public async Task CastFateAsync(string args)
        {
            FateCaster caster = new(Config.Instance.FateConfig);

            int quantity = args.SetThrowQuantity();
            int actTAMV = args.SetThrowAgainMinValue();
            DiceType actDC = args.SetDiceTypeValue();
            int actDF = args.SetThrowDifficulty();
            bool actI = args.IsInspired();
            bool actR = args.IsRoted();
            int actQ = quantity > 0 ? quantity : 1;
            bool actTA = quantity > 0 && Config.Instance.DefaultCast.ThrowAgain;

            PathPool pathPool = new(
                diceType: actDC, 
                quantity: actQ,
                difficulty: actDF,
                throwAgain: actTA,
                throwAgainMinValue: actTAMV,
                isRote: actR
            );

            ResultPath resultPath = caster.CastFate(pathPool);
            SocketUser currentUser = Context.User;

            int successes = resultPath.Successes(actDF);
            string endResult = "Failure";
            int excLimit = actI ? 3 : 5;
            if (successes > 0)
                endResult = successes < excLimit ? "Success" : "Exceptional Success";

            if(!(quantity > 0))
            {
                successes = resultPath.Successes((int)actDC);
                int failures = resultPath.Failures(Config.Instance.DefaultCast.Threshold);
                int mRes = successes - failures;
                endResult = mRes switch
                {
                    -1 => "Dramatic Failure",
                    1 => "Success",
                    _ => "Failure"
                };
            }

            string result = $"{currentUser.Username} => Final Pool: {resultPath.ResultsPool} Successes: {successes} Result: {endResult}. {resultPath.ResultsString}";

            await ReplyAsync(result);
        }

        
    }
}
