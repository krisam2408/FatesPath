using System;
using System.Collections.Generic;

namespace FatesPathLib;

public sealed class FateCaster
{
    private readonly Random m_random = new();

    public ResultPath CastFate(params PathPool[] pool)
    {
        ResultPath output = new();
        foreach (PathPool path in pool)
        {
            Dice[] singlePath = CastSinglePath(path);
            output.Results.AddRange(singlePath);
        }

        return output;
    }

    private Dice[] CastSinglePath(PathPool path)
    {
        List<Dice> output = new();
        List<Dice> firstRoll = new();
        List<Dice> roteRoll = new();
        List<Dice> againRoll = new();

        for(int i = 0; i < path.Quantity; i++)
        {
            int pick = m_random.Next((int)path.Dice) + 1;
            firstRoll.Add(new(path.Dice, pick));
        }

        if(path.IsRote)
        {
            int failures = 0;
            foreach (Dice d in firstRoll)
                if (d.IsFailure(path.ThrowDifficulty))
                    failures++;

            if(failures > 0)
            {
                PathPool rotePathPool = new(path.Dice, failures, path.ThrowDifficulty, false, 0, false);
                Dice[] roteResults = CastSinglePath(rotePathPool);
                roteRoll.AddRange(roteResults);
            }
        }

        if(path.ThrowAgain)
        {
            int againRolls = 0;
            foreach(Dice d in firstRoll)
                if(d.ThrowAgain(path.ThrowAgainMinValue))
                    againRolls++;

            if(againRolls > 0)
            {
                PathPool againPathPool = new(path.Dice, againRolls, path.ThrowDifficulty, true, path.ThrowAgainMinValue, false);
                Dice[] againResults = CastSinglePath(againPathPool);
                againRoll.AddRange(againResults);
            }
        }

        output.AddRange(firstRoll);
        output.AddRange(roteRoll);
        output.AddRange(againRoll);

        return output.ToArray();
    }
}
