using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatesPathLib.DataTransfer;

public sealed class DiceComparer : IComparer<Dice>
{
    public int Compare(Dice x, Dice y)
    {
        if (x.DiceType != y.DiceType)
            return CompareByDiceType(x, y);
        if (x.Result != y.Result)
            return CompareByResult(x,y);
        return 0;
    }

    private int CompareByResult(Dice x, Dice y) => x.Result - y.Result;
    
    private int CompareByDiceType(Dice x, Dice y) => (int)x.DiceType - (int)y.DiceType;
}
