using FatesPathLib.DataTransfer;
using System.Collections.Generic;
using System.Linq;

namespace FatesPathLib
{
    public class ResultPath
    {
        public List<Dice> Results { get;  set; }

        public List<Dice> OrderedResults { get { return Results.OrderBy(d => d, new DiceComparer()).ToList(); } }

        public int[] RawResults { get { return Results.Select(d => d.Result).ToArray(); } }

        public int ResultsPool { get { return Results.Count; } }

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
                string output = string.Empty;
                foreach (Dice d in OrderedResults)
                    output += $"• {d.Result} •";

                output = output.Replace("••", "•");

                return output.Trim();
            }
        }

        public ResultPath()
        {
            Results = new();
        }

        public int Successes(int difficulty)
        {
            int output = 0;

            foreach (Dice d in Results)
                if (d.IsSuccess(difficulty))
                    output++;

            return output;
        }

        public int Failures(int threshold)
        {
            int output = 0;

            foreach (Dice d in Results)
                if (d.IsFailure(threshold))
                    output++;

            return output;
        }

        public int ThrowAgain(int minValue)
        {
            int output = 0;

            foreach (Dice d in Results)
                if (d.ThrowAgain(minValue))
                    output++;

            return output;
        }
    }
}
