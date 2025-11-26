using FatesPathLib.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FatesPathLib;

public sealed class ResultPath
{
    public List<Dice> Results { get;  set; }

    public List<Dice> OrderedResults => Results.OrderBy(d => d, new DiceComparer()).ToList();

    public int[] RawResults => Results.Select(d => d.Result).ToArray();

    public int ResultsPool => Results.Count;

    public int ResultsSum
    {
        get
        {
            int output = 0;
            foreach (Dice d in Results)
                output += d.Result;
            return output;
        }
    }

    public string ResultsString
    {
        get
        {
            string output = "";
            foreach (Dice d in OrderedResults)
                output += $"• {d.Result} •";

            output = output.Replace("••", "•");

            return output.Trim();
        }
    }

    public ResultPath()
    {
        Results = [];
    }

    private int Count(int value, Func<Dice, int, bool> predicate)
    {
        int output = 0;

        foreach (Dice d in Results)
            if (predicate(d, value))
                output++;

        return output;
    }

    public int Successes(int difficulty) => Count(difficulty, (d, v) => d.IsSuccess(v));

    public int Failures(int threshold) => Count(threshold, (d, v) => d.IsFailure(v));

    public int ThrowAgain(int minValue) => Count(minValue, (d, v) => d.ThrowAgain(v));
}
