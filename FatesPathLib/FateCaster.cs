using FatesPathLib.Configuration;
using System;
using System.Collections.Generic;

namespace FatesPathLib;

public sealed class FateCaster
{
    private int TensorY { get; set; }
    private int TensorZ { get; set; }

    public FateCaster(int y = 1, int z = 1)
    {
        TensorY = y;
        TensorZ = z;
    }

    public FateCaster(FateConfig config)
    {
        TensorY = config.TensorY;
        TensorZ = config.TensorZ;
    }

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

        Random ran = new();
        int[,,] preliminarResult = new int[path.Quantity,TensorY,TensorZ];

        for(int z = 0; z < TensorZ; z++)
            for(int y = 0; y < TensorY;y++)
                for(int x = 0; x < path.Quantity; x++)
                {
                    int r = ran.Next((int)path.Dice) + 1;
                    preliminarResult[x, y, z] = r;
                }

        for(int pickX = 0; pickX < path.Quantity; pickX++)
        {
            int pickZ = ran.Next(TensorZ);
            int pickY = ran.Next(TensorY);
            firstRoll.Add(new(path.Dice, preliminarResult[pickX, pickY, pickZ]));
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
                roteRoll.AddRange(CastSinglePath(rotePathPool));
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
                againRoll.AddRange(CastSinglePath(againPathPool));
            }
        }

        output.AddRange(firstRoll);
        output.AddRange(roteRoll);
        output.AddRange(againRoll);

        return output.ToArray();
    }
}
