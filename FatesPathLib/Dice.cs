using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatesPathLib
{
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

        public bool IsSuccess(int difficulty)
        {
            if (Result >= difficulty)
                return true;
            return false;
        }

        public bool IsFailure(int threshold)
        {
            if (Result <= threshold)
                return true;
            return false;
        }

        public bool ThrowAgain(int minValue)
        {
            if (Result >= minValue)
                return true;
            return false;
        }

        public override string ToString()
        {
            return $"{Result}/{(int)DiceType}";
        }
    }
}
