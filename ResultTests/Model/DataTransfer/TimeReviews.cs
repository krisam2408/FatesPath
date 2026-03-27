namespace ResultTests.Model.DataTransfer;

public class TimeReviews
{
    public required Dictionary<int, TimeSpan> Results { get; set; }

    public static TimeReviews Create()
    {
        TimeReviews result = new() 
        { 
            Results = new()
        };

        for(int i = 1; i <= 9; i++)
        {
            int key = (int)Math.Pow(10, i);
            result.Results.Add(key, TimeSpan.Zero);
        }

        return result;
    }

    public PlainTimeReview ToPlain()
    {
        int getKey(int exp) => (int)Math.Pow(10, exp);

        return new()
        {
            Power1 = Results[getKey(1)],
            Power2 = Results[getKey(2)],
            Power3 = Results[getKey(3)],
            Power4 = Results[getKey(4)],
            Power5 = Results[getKey(5)],
            Power6 = Results[getKey(6)],
            Power7 = Results[getKey(7)],
            Power8 = Results[getKey(8)],
            Power9 = Results[getKey(9)],
        };
    }
}
