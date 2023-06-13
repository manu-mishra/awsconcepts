using Application.Scraper.AwsSearch;
using System.Diagnostics;

public static class DirectoryScraper
{
    public static async Task<List<Record>> ScrapeEntireDirectory()
    {
        var descendingResults = await SearchDirectory.GetSearchResultsAsync("blog-posts", 1000, 1000, "desc");
        var ascendingResults = await SearchDirectory.GetSearchResultsAsync("blog-posts", 1000, 1000, "asc");

        var smallestDate = GetSmallestDate(descendingResults);
        var largestDate = GetLargestDate(ascendingResults);

        Debug.WriteLine($"Smallest Date: {smallestDate}");
        Debug.WriteLine($"Largest Date: {largestDate}");

        var allResults = await FetchResultsByDateRange("blog-posts", 1000, 1000, smallestDate, largestDate);

        var combinedResults = MergeResults(descendingResults, ascendingResults, allResults);

        return combinedResults;
    }

    private static DateTime GetSmallestDate(List<Record> descendingResults)
    {
        var smallestDate = descendingResults.Min(record => record.Item.DateCreated);
        Debug.WriteLine($"Smallest Date: {smallestDate}");
        return smallestDate;
    }

    private static DateTime GetLargestDate(List<Record> ascendingResults)
    {
        var largestDate = ascendingResults.Max(record => record.Item.DateCreated);
        Debug.WriteLine($"Largest Date: {largestDate}");
        return largestDate;
    }

    private static async Task<List<Record>> FetchResultsByDateRange(string directoryId, int sizeOfRecordsPerRequest, int maxNumberPages, DateTime startDate, DateTime endDate)
    {
        Debug.WriteLine($"Fetching results from {startDate} to {endDate}");
        var allResults = new List<Record>();
        var currentDate = startDate;

        while (currentDate <= endDate)
        {
            Debug.WriteLine($"Fetching results for date: {currentDate}");
            var dateResults = await SearchDirectory.GetSearchResultsByCreatedDateAsync(directoryId, sizeOfRecordsPerRequest, maxNumberPages, currentDate);
            allResults.AddRange(dateResults);

            currentDate = currentDate.AddDays(1);
        }

        return allResults;
    }

    private static List<Record> MergeResults(List<Record> descendingResults, List<Record> ascendingResults, List<Record> allResults)
    {
        Debug.WriteLine($"Merging results: Descending: {descendingResults.Count}, Ascending: {ascendingResults.Count}, All: {allResults.Count}");

        var uniqueResults = new HashSet<Record>(descendingResults, new RecordEqualityComparer());

        uniqueResults.UnionWith(ascendingResults);
        uniqueResults.UnionWith(allResults);

        return uniqueResults.ToList();
    }

    private class RecordEqualityComparer : IEqualityComparer<Record>
    {
        public bool Equals(Record x, Record y)
        {
            return x.Item.Id == y.Item.Id;
        }

        public int GetHashCode(Record obj)
        {
            return obj.Item.Id.GetHashCode();
        }
    }
}
