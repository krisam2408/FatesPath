using DiscordBot.Services.ConfigurationLib;
using FatesPathLib;
using System.Text.RegularExpressions;

namespace DiscordBot.Extensions
{
    public static class CommandExtension
    {
        public static int SetThrowQuantity(this string args)
        {
            Regex regex = new(@"[0-9]+");
            Match match = regex.Match(args);
            if (match.Success)
            {
                string num = match.Value;
                return int.Parse(num);
            }

            return 0;

        }

        public static int SetThrowAgainMinValue(this string args)
        {
            Regex regex = new(@"([Tt][Aa][0-9]+)");
            Match match = regex.Match(args);
            if (match.Success)
            {
                string num = match.Value.Remove(0, 2);
                int val = int.Parse(num);
                val = val > (int)Config.Instance.DefaultCast.DiceType ? (int)Config.Instance.DefaultCast.DiceType : val;
                val = val > 1 ? val : 2;
                return val;
            }

            return Config.Instance.DefaultCast.ThrowAgainMinValue;
        }

        public static DiceType SetDiceTypeValue(this string args)
        {
            Regex regex = new(@"([Dd][Ii][0-9]+)");
            Match match = regex.Match(args);
            if (match.Success)
            {
                string num = match.Value.Remove(0, 2);
                int val = int.Parse(num);
                val = val > 0 ? val : 2;
                DiceType output = (DiceType)val;
                return output;
            }

            return Config.Instance.DefaultCast.DiceType;
        }

        public static int SetThrowDifficulty(this string args)
        {
            Regex regex = new(@"([Dd][Ff][0-9]+)");
            Match match = regex.Match(args);
            if (match.Success)
            {
                string num = match.Value.Remove(0, 2);
                int val = int.Parse(num);
                val = val > (int)Config.Instance.DefaultCast.DiceType ? (int)Config.Instance.DefaultCast.DiceType : val;

                return val;
            }

            DiceType diceType = args.SetDiceTypeValue();

            if (diceType != Config.Instance.DefaultCast.DiceType)
            {
                int diceValue = (int)diceType;
                double defaultSuccessRate = 0.7;
                for (int throwValue = 1; throwValue < diceValue; throwValue++)
                {
                    double result = (double)throwValue / (double)diceValue;
                    if (result > defaultSuccessRate)
                        return throwValue;
                }
            }

            return Config.Instance.DefaultCast.Difficulty;
        }

        public static bool IsInspired(this string args)
        {
            Regex regex = new(@"(-[Ii])");
            Match match = regex.Match(args);
            if (match.Success)
                return true;
            return false;
        }

        public static bool IsRoted(this string args)
        {
            Regex regex = new(@"(-[Rr])");
            Match match = regex.Match(args);
            if (match.Success)
                return true;
            return false;
        }
    }
}
