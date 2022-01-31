
namespace FatesPathLib
{
    public struct PathPool
    {
        public DiceType Dice { get; private set; }
        public int Quantity { get; private set; }
        public bool IsRote { get; private set; }
        public int ThrowDifficulty { get; private set; }
        public bool ThrowAgain { get; private set; }
        public int ThrowAgainMinValue { get; private set; }

        public PathPool(int diceFaces, int quantity, int difficulty = 4, bool throwAgain = false, int throwAgainMinValue = 0, bool isRote = false)
        {
            Dice = (DiceType)diceFaces;
            Quantity = quantity;
            ThrowDifficulty = difficulty;
            ThrowAgain = throwAgain;
            ThrowAgainMinValue = diceFaces;
            IsRote = isRote;

            if(throwAgainMinValue > 0)
            {
                ThrowAgainMinValue = throwAgainMinValue;
                if (throwAgainMinValue > diceFaces)
                    ThrowAgainMinValue = diceFaces;
            }

            if (!throwAgain)
                ThrowAgainMinValue = 0;
        }

        public PathPool(DiceType diceType, int quantity, int difficulty = 4, bool throwAgain = false, int throwAgainMinValue = 0, bool isRote = false)
        {
            Dice = diceType;
            Quantity = quantity;
            ThrowDifficulty = difficulty;
            ThrowAgain = throwAgain;
            int diceFaces = (int)diceType;
            ThrowAgainMinValue = diceFaces;
            IsRote = isRote;

            if (throwAgainMinValue > 0)
            {
                ThrowAgainMinValue = throwAgainMinValue;
                if (throwAgainMinValue > diceFaces)
                    ThrowAgainMinValue = diceFaces;
            }

            if (!throwAgain)
                ThrowAgainMinValue = 0;
        }

        public override string ToString()
        {
            return $"{Quantity}{Dice}";
        }

        public override bool Equals(object obj)
        {
            if(obj is PathPool path)
                if (path.Dice == Dice)
                    return true;

            return true;
        }

        public override int GetHashCode()
        {
            return (int)Dice * Quantity;
        }
    }
}
