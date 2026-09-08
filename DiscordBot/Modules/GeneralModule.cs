using Discord;
using Discord.WebSocket;
using FatesPathLib;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules;

internal static class GeneralModule
{
    public static async Task Roll(SocketSlashCommand cmd)
    {
        long longQ = (long)cmd
            .Data
            .Options
            .Where(o => o.Name == "cantidad")
            .First()
            .Value;

        int quantity = (int)Math.Clamp(longQ, 0, 100);

        long longF = (long)cmd
            .Data
            .Options
            .Where(o => o.Name == "caras")
            .First()
            .Value;

        int faces = (int)Math.Clamp(longF, 0, 100);

        FateCaster caster = new();

        DiceType dice = (DiceType)faces;

        PathPool pool = new(dice, quantity);
        ResultPath path = caster.CastFate(pool);

        EmbedBuilder eb = new EmbedBuilder()
            .WithAuthor(cmd.User.GlobalName, cmd.User.GetAvatarUrl() ?? cmd.User.GetDefaultAvatarUrl())
            .WithTitle($"lanzó {quantity}d{faces}:")
            .WithDescription(path.ResultsString)
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        await cmd.RespondAsync(embed: eb.Build());
    }

    public static async Task Coin(SocketSlashCommand cmd)
    {
        FateCaster caster = new();
        string currentUser = cmd.User.GlobalName;

        DiceType dice = DiceType.Coin;

        PathPool pool = new(dice, 1);
        ResultPath result = caster.CastFate(pool);

        int diceResult = result.Results[0].Result;

        string reply = diceResult == 1 ? "SELLO" : "CARA";

        EmbedBuilder eb = new EmbedBuilder()
            .WithAuthor(cmd.User.GlobalName, cmd.User.GetAvatarUrl() ?? cmd.User.GetDefaultAvatarUrl())
            .WithTitle("lanzó una moneda:")
            .WithDescription(reply)
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        await cmd.RespondAsync(reply);
    }

}
