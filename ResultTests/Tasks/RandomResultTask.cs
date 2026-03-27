using CsvHelper;
using ResultTests.Model;
using ResultTests.Model.DataTransfer;
using System;
using System.Diagnostics;
using System.Globalization;
using TerminalWrapper;

namespace ResultTests.Tasks.Random;

public class RandomResultTask : MainTask
{
    public override string TaskName => $"Throw {m_throws} times (D{m_diceFaces})";

    private readonly int m_diceFaces;
    private readonly int m_power;
    private readonly int m_throws;
    private readonly string m_outputPath;

    public RandomResultTask(int diceFaces, int throws, string path)
    {
        m_diceFaces = diceFaces;
        m_power = throws;
        m_throws = (int)Math.Pow(10, throws);
        m_outputPath = path;
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

        System.Random random = new();

        for(int i = 0; i < m_throws; i++)
        {
            int face = random.Next(0, m_diceFaces) + 1;
            result[face]++;
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
