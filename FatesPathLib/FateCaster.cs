using FatesPathLib.Configuration;
using System;
using System.Collections.Generic;

namespace FatesPathLib
{
    public class FateCaster
    {
        private int TensorY { get; set; }
        private int TensorZ { get; set; }

        public int[] BaseMatrix { get { return new int[] { TensorY, TensorZ }; } }

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
                output.Add(new(path.Dice, preliminarResult[pickX, pickY, pickZ]));
            }

            if(path.ThrowAgain)
            {
                List<Dice> checkPool = output;

                int throwAgain;
                do
                {
                    List<Dice> addedPool = new();
                    throwAgain = 0;
                    foreach (Dice d in checkPool)
                        if (d.ThrowAgain(path.ThrowAgainMinValue))
                            throwAgain++;

                    for (int i = 0; i < throwAgain; i++)
                    {
                        int pickZ = ran.Next(TensorZ);
                        int pickY = ran.Next(TensorY);
                        int pickX = ran.Next(path.Quantity);
                        addedPool.Add(new(path.Dice, preliminarResult[pickX, pickY, pickZ]));
                    }

                    output.AddRange(addedPool);
                    checkPool = addedPool;

                } while (throwAgain > 0);
            }

            if(path.IsRote)
            {
                List<Dice> addedPool = new();

                int throwAgain = 0;
                foreach (Dice d in output)
                    if (d.IsFailure(path.ThrowDifficulty))
                        throwAgain++;

                for(int i = 0;i < throwAgain;i++)
                {
                    int pickZ = ran.Next(TensorZ);
                    int pickY = ran.Next(TensorY);
                    int pickX = ran.Next(path.Quantity);
                    addedPool.Add(new(path.Dice, preliminarResult[pickX, pickY, pickZ]));
                }

                output.AddRange(addedPool);
            }

            return output.ToArray();
        }
    }
}
