using Discord;
using Discord.WebSocket;
using FatesPathLib;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules;

internal static class CoDModule
{
    public static async Task Cast(SocketSlashCommand cmd)
    {
        long longQ = (long)cmd
            .Data
            .Options
            .Where(o => o.Name == "cantidad")
            .First()
            .Value;

        int quantity = (int)Math.Clamp(longQ, 0, 100);
        
        SocketSlashCommandDataOption boolI = cmd
            .Data
            .Options
            .Where(o => o.Name == "inspirado")
            .FirstOrDefault();

        bool isInspired = boolI == null ? false : (bool)boolI.Value;

        SocketSlashCommandDataOption boolR = cmd
            .Data
            .Options
            .Where(o => o.Name == "rutinaria")
            .FirstOrDefault();

        bool isRoted = boolR == null ? false : (bool)boolR.Value;

        FateCaster caster = new();

        if (quantity == 0)
        {
            await CastZero(cmd);
            return;
        }

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
        Success pathResult = CheckResult(isInspired, successes);

        string result = pathResult switch
        {
            Success.Exceptional => "Éxito Excepcional",
            Success.Success => "Éxito",
            Success.DramaticFailure => "Fallo Dramático",
            _ => "Fallo"
        };

        Color color = pathResult switch
        {
            Success.Exceptional or Success.Success => Color.Green,
            _ => Color.Red
        };

        string reply = $"""
            Tirada Final: {path.ResultsPool}
            Éxitos: {successes}
            Resultado: {result}
            {path.ResultsString}
            """;

        EmbedBuilder eb = new EmbedBuilder()
            .WithAuthor(cmd.User.GlobalName, cmd.User.GetAvatarUrl() ?? cmd.User.GetDefaultAvatarUrl())
            .WithTitle($"lanzó {path.ResultsPool} dados:")
            .WithDescription(reply)
            .WithColor(color)
            .WithCurrentTimestamp();

        await cmd.RespondAsync(embed: eb.Build());
    }

    private static async Task CastZero(SocketSlashCommand cmd)
    {
        FateCaster caster = new();

        PathPool pool = new(
            diceType: DiceType.D10,
            quantity: 1,
            difficulty: 10,
            throwAgain: false
        );

        ResultPath path = caster.CastFate(pool);

        int successes = path.Successes(10);
        int failures = path.Failures(1);
        Success pathResult = CheckResult(false, successes, failures);

        string result = pathResult switch
        {
            Success.Exceptional => "Éxito Excepcional",
            Success.Success => "Éxito",
            Success.DramaticFailure => "Fallo Dramático",
            _ => "Fallo"
        };

        Color color = pathResult switch
        {
            Success.Exceptional or Success.Success => Color.Green,
            _ => Color.Red
        };

        string reply = $"""
            Tirada Final: {path.ResultsPool}
            Éxitos: {successes}
            Fallos: {failures}
            Resultado: {result}
            {path.ResultsString}
            """;

        EmbedBuilder eb = new EmbedBuilder()
            .WithAuthor(cmd.User.GlobalName, cmd.User.GetAvatarUrl() ?? cmd.User.GetDefaultAvatarUrl())
            .WithTitle($"lanzó 1 dado:")
            .WithDescription(reply)
            .WithColor(color)
            .WithCurrentTimestamp();

        await cmd.RespondAsync(embed: eb.Build());
    }

    private static Success CheckResult(bool inspired, int successes, int failures = 0)
    {
        int exceptionalLimit = 5;
        if (inspired)
            exceptionalLimit = 3;

        if (successes >= exceptionalLimit)
            return Success.Exceptional;

        if (successes > 0)
            return Success.Success;

        bool dramatic = failures > 0;

        if(!dramatic)
            return Success.Failure;

        int mRes = successes - failures;
        if (mRes < 0)
            return Success.DramaticFailure;

        return Success.Failure;
    }
}
