namespace Application.Scraper.AwsSearch;

public static class SearchDirectory
{
    public static async Task<List<Record>> GetSearchResultsAsync(
        string directoryId, int sizeOfRecordsPerRequest, int maxNumberPages,string sortOrder="desc")
    {
        string baseUrl = "https://aws.amazon.com/api/dirs/items/search";
        int page = 0;
        var totalFetchCount = 0;
        bool hasMoreData = true;
        List<Record> results = new List<Record>();

        while (hasMoreData && page < maxNumberPages)
        {
            string url = baseUrl.SetQueryParam("item.directoryId", directoryId)
                                .SetQueryParam("sort_by", "item.additionalFields.createdDate")
                                .SetQueryParam("sort_order", sortOrder)
                                .SetQueryParam("size", sizeOfRecordsPerRequest)
                                .SetQueryParam("item.locale", "en_US")
                                .SetQueryParam("page", page);

            try
            {
                var response = await url.GetJsonAsync<SearchResponse>();
                if (response.Items.Count == 0)
                {
                    hasMoreData = false;
                    continue;
                }

                totalFetchCount = totalFetchCount + response.Items.Count;
                results.AddRange(response.Items);

                Console.Write($"\rFetched {response.Items.Count} for page {page} with total {totalFetchCount}");

                page++;
                await Task.Delay(200);
            }
            catch (FlurlHttpException ex)
            {
                Console.WriteLine("An error occurred while making the request:");
                Console.WriteLine(ex.Message);
                hasMoreData = false;
            }
        }

        Console.WriteLine($"{totalFetchCount} data has been fetched");
        return results;
    }



}