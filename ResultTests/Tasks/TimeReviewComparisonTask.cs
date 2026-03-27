using CsvHelper;
using ResultTests.Model;
using ResultTests.Model.DataTransfer;
using System.Globalization;
using System.IO;
using TerminalWrapper;

namespace ResultTests.Tasks;

public class TimeReviewComparisonTask : MainTask
{
    public override string TaskName => "Time Review Comparison";

    private readonly string m_outputPath;
    private readonly string[] m_folders =
    [
        "Random",
        "Fate1x1",
        "Fate3x3",
        "Fate5x5",
        "Fate10x10",
        "Fate15x15",
    ];

    public TimeReviewComparisonTask(string outputPath)
    {
        m_outputPath = outputPath;
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        Dictionary<string, PlainTimeReview> reviews = [];
        foreach(string folder in m_folders)
        {
            string reviewPath = $"{m_outputPath}/{folder}";
            TimeReviews review = await FileManagement.GetReviews(reviewPath);
            PlainTimeReview plain = review.ToPlain();
            reviews.Add(folder, plain);
        }

        string path = $"{m_outputPath}/timeComparison.csv";
        using StreamWriter sw = new(path);
        using CsvWriter csv = new(sw, CultureInfo.InvariantCulture);
        csv.WriteRecords(reviews);
    }
}
