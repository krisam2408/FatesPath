using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatesPathLib;

public struct Dice
{
    public DiceType DiceType { get; private set; }
    public int Result { get; private set; }

    public bool IsMaxValue
    {
        get
        {
            if (Result == (int)DiceType)
                return true;
            return false;
        }
    }

    public Dice(int diceFaces, int result)
    {
        DiceType = (DiceType)diceFaces;
        Result = result;
    }

    public Dice(DiceType diceType, int result)
    {
        DiceType = diceType;
        Result = result;
    }

    public bool IsSuccess(int difficulty) => Result >= difficulty;

    public bool IsFailure(int threshold) => Result <= threshold;

    public bool ThrowAgain(int minValue) => Result >= minValue;

    public override string ToString() => $"{Result}/{(int)DiceType}";
}
