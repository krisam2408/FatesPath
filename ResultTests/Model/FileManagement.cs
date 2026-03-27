using ResultTests.Model.DataTransfer;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResultTests.Model;

public static class FileManagement
{
    public static async Task<TimeReviews> GetReviews(string path)
    {
        path = $"{path}/timeReviews.json";
        
        string json;
        TimeReviews? review;

        if(!File.Exists(path))
        {
            review = TimeReviews.Create();

            json = JsonSerializer.Serialize(review);
            await File.WriteAllTextAsync(path, json);

            return review;
        }

        json = await File.ReadAllTextAsync(path);
        review = JsonSerializer.Deserialize<TimeReviews>(json);

        if (review is null)
            throw new NullReferenceException();

        return review;
    }

    public static async Task SaveReview(string path, TimeReviews review)
    {
        path = $"{path}/timeReviews.json";

        string json = JsonSerializer.Serialize(review);
        await File.WriteAllTextAsync(path, json);
    }
}
