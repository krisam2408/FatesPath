using CsvHelper;
using FatesPathLib;
using ResultTests.Model;
using ResultTests.Model.DataTransfer;
using System;
using System.Diagnostics;
using System.Globalization;
using TerminalWrapper;

namespace ResultTests.Tasks;

public class FateResultTask : MainTask
{
    public override string TaskName => $"Throw {m_throws} times (D{m_diceFaces})";

    private readonly int m_diceFaces;
    private readonly int m_power;
    private readonly int m_throws;
    private readonly string m_outputPath;
    private readonly int m_tensorY;
    private readonly int m_tensorZ;

    public FateResultTask(int diceFaces, int power, int y, int z, string outputPath)
    {
        m_diceFaces = diceFaces;
        m_power = power;
        m_throws = (int)Math.Pow(10, power);
        m_outputPath = outputPath;
        m_tensorY = y;
        m_tensorZ = z;
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        string path = $"{m_outputPath}/result_t10p{m_power}_d{m_diceFaces}.csv";
        TimeReviews review = await FileManagement.GetReviews(m_outputPath);

        Dictionary<int, long> result = new();
        for (int i = 1; i <= m_diceFaces; i++)
            result.Add(i, 0);

        Stopwatch watch = new();
        watch.Start();

        FateCaster caster = new();

        PathPool pool = new(
            diceType: (DiceType)m_diceFaces,
            quantity: m_throws
        );

        ResultPath resultPath = caster.CastFate(pool);

        foreach(Dice d in resultPath.Results)
        {
            result[d.Result]++;
        }

        watch.Stop();

        using StreamWriter sw = new(path);
        using CsvWriter csv = new(sw, CultureInfo.InvariantCulture);
        csv.WriteRecords(result);

        TimeSpan time = watch.Elapsed;
        review.Results[m_throws] = time;

        await FileManagement.SaveReview(m_outputPath, review);
    }
}
