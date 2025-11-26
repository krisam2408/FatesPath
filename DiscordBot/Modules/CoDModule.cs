using Discord.WebSocket;
using FatesPathLib;
using FatesPathLib.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Modules;

internal class CoDModule : BaseModule
{
    public string[] Cast(SocketUserMessage message, FateConfig context)
    {
        FateCaster caster = new(context);
        string[] args = GetArguments(message);

        if(args.Length == 0)
            return ["Invalid command format"];

        List<string> result = [];

        SocketUser currentUser = message.Author;

        if (!int.TryParse(args[0], out int quantity))
            quantity = 0;

        int originalQuantity = quantity;
        if (originalQuantity < 0)
            originalQuantity = 0;

        if (quantity == 0)
            quantity = 1;

        bool isInspired = args.Contains("-i");
        bool isRoted = args.Contains("-r");

        PathPool pool = new(
            diceType: DiceType.D10, 
            quantity: quantity,
            difficulty: 8,
            throwAgain: true,
            throwAgainMinValue: 10,
            isRote: isRoted
        );

        ResultPath path = caster.CastFate(pool);

        int successes = path.Successes(8);

        string checkResult()
        {
            int exceptionalLimit = 5;
            if (isInspired)
                exceptionalLimit = 3;

            if (successes > exceptionalLimit)
                return "Exceptional Success";

            if (successes > 0)
                return "Success";

            return "Failure";
        }

        string endResult = checkResult();
        
        if(originalQuantity == 0)
        {
            successes = path.Successes(10);
            int failures = path.Failures(1);
            int mRes = successes - failures;
            endResult = mRes switch
            {
                -1 => "Dramatic Failure",
                1 => "Success",
                _ => "Failure"
            };
        }

        string reply = $"{currentUser.Username} => Final Pool: {path.ResultsPool} Successes: {successes} Result: {endResult}. {path.ResultsString}";

        return [reply];
    }

    
}
