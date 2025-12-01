using Discord.WebSocket;
using FatesPathLib;
using FatesPathLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Modules;

internal class GeneralModule : BaseModule
{
    public string[] Roll(SocketUserMessage message, FateConfig context)
    {
        string[] args = GetArguments(message);

        if (args.Length == 0)
            return ["Formato de comando inválido"];

        SocketUser currentUser = message.Author;
        FateCaster caster = new(context);
        List<string> result = [];

        foreach (string arg in args)
        {
            try
            {
                int[] parameters = arg.Trim()
                    .Split('d')
                    .Select(i => int.Parse(i))
                    .ToArray();

                DiceType dice = (DiceType)parameters[1];

                PathPool pool = new(dice, parameters[0]);
                ResultPath path = caster.CastFate(pool);

                string reply = $"{currentUser.Username} lanzó {arg.ToUpper()}: {path.ResultsString}";

                result.Add(reply);

            }
            catch (Exception)
            {
                result.Add("Formato de comando inválido");
            }
        }

        return result.ToArray();
    }

    public string[] Coin(SocketUserMessage message, FateConfig context)
    {
        FateCaster caster = new(context);
        SocketUser currentUser = message.Author;

        DiceType dice = DiceType.Coin;

        PathPool pool = new(dice, 1);
        ResultPath result = caster.CastFate(pool);

        int diceResult = result.Results[0].Result;

        string reply = diceResult == 1 ? $"{currentUser.Username} lanzó NO" : $"{currentUser.Username} lanzó SÍ";

        return [ reply ];
    }

}
