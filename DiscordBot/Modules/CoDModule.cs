using Discord.WebSocket;
using FatesPathLib;
using FatesPathLib.Configuration;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DiscordBot.Modules;

internal class CoDModule : BaseModule
{
    public string[] Cast(SocketUserMessage message, FateConfig context)
    {
        string[] args = GetArguments(message);

        if(args.Length == 0)
            return ["Formato de comando inválido"];

        List<string> result = [];

        FateCaster caster = new(context);
        SocketUser currentUser = message.Author;

        if (!int.TryParse(args[0], out int quantity))
            return [];

        if (quantity == 0)
            return CastZero(message, context);

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
        string pathResult = CheckResult(isInspired, successes);

        string reply = $"""
            {currentUser.Username} tira =>
            Tirada Final: {path.ResultsPool}
            Éxitos: {successes}
            Resultado: {pathResult}
            {path.ResultsString}
            """;
            
        return [reply];
    }

    private string[] CastZero(SocketUserMessage message, FateConfig context)
    {
        FateCaster caster = new(context);
        SocketUser currentUser = message.Author;

        PathPool pool = new(
            diceType: DiceType.D10,
            quantity: 1,
            difficulty: 10,
            throwAgain: false
        );

        ResultPath path = caster.CastFate(pool);

        int successes = path.Successes(10);
        int failures = path.Failures(1);
        string pathResult = CheckResult(false, successes, failures);

        string reply = $"""
            {currentUser.Username} tira =>
            Tirada Final: {path.ResultsPool}
            Éxitos: {successes}
            Fallos: {failures}
            Resultado: {pathResult}
            {path.ResultsString}
            """;

        return [reply];
    }

    private string CheckResult(bool inspired, int successes, int failures = 0)
    {
        int exceptionalLimit = 5;
        if (inspired)
            exceptionalLimit = 3;

        if (successes >= exceptionalLimit)
            return "Éxito Excepcional";

        if (successes > 0)
            return "Éxito";

        bool dramatic = failures > 0;

        if(!dramatic)
            return "Fallo";

        int mRes = successes - failures;
        if (mRes < 0)
            return "Fallo Dramático";

        return "Fallo";
    }
}
