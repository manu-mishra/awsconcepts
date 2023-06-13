using System.Diagnostics;

namespace Application.Scraper.AwsSearch
{
    public static class SearchDirectory
    {
        private const string BaseUrl = "https://aws.amazon.com/api/dirs/items/search";
        private const int DelayMilliseconds = 100;

        public static async Task<List<Record>> GetSearchResultsAsync(
            string directoryId, int sizeOfRecordsPerRequest, int maxNumberPages, string sortOrder = "desc")
        {
            int page = 0;
            int totalFetchCount = 0;
            bool hasMoreData = true;
            List<Record> results = new List<Record>();

            while (hasMoreData && page < maxNumberPages)
            {
                var url = BuildSearchUrl(directoryId, sizeOfRecordsPerRequest, sortOrder, page);

                try
                {
                    await Task.Delay(DelayMilliseconds);
                    var response = await url.GetJsonAsync<SearchResponse>();
                    if (response.Items.Count == 0)
                    {
                        hasMoreData = false;
                        continue;
                    }

                    totalFetchCount += response.Items.Count;
                    results.AddRange(response.Items);

                    Debug.WriteLine($"Fetched {response.Items.Count} for page {page} with total {totalFetchCount}");

                    page++;
                   
                }
                catch (FlurlHttpException ex)
                {
                    Debug.WriteLine("An error occurred while making the request:");
                    Debug.WriteLine(ex.Message);
                    hasMoreData = false;
                }
            }

            Debug.WriteLine($"{totalFetchCount} data has been fetched");
            return results;
        }

        public static async Task<List<Record>> GetSearchResultsByCreatedDateAsync(
            string directoryId, int sizeOfRecordsPerRequest, int maxNumberPages, DateTime createdDate)
        {
            int page = 0;
            int totalFetchCount = 0;
            bool hasMoreData = true;
            List<Record> results = new List<Record>();

            while (hasMoreData && page < maxNumberPages)
            {
                var url = BuildSearchUrl(directoryId, sizeOfRecordsPerRequest, "desc", page);
                url.SetQueryParam("item.additionalFields.createdDate", createdDate.ToString("yyyy-MM-dd"));

                try
                {
                    await Task.Delay(DelayMilliseconds);
                    var response = await url.GetJsonAsync<SearchResponse>();
                    if (response.Items.Count == 0)
                    {
                        hasMoreData = false;
                        continue;
                    }

                    totalFetchCount += response.Items.Count;
                    results.AddRange(response.Items);

                    Debug.WriteLine($"Fetched {response.Items.Count} for page {page} with total {totalFetchCount}");

                    page++;
                }
                catch (FlurlHttpException ex)
                {
                    Debug.WriteLine("An error occurred while making the request:");
                    Debug.WriteLine(ex.Message);
                    hasMoreData = false;
                }
            }

            Debug.WriteLine($"{totalFetchCount} data has been fetched");
            return results;
        }

        private static Url BuildSearchUrl(string directoryId, int sizeOfRecordsPerRequest, string sortOrder, int page)
        {
            return BaseUrl
                .SetQueryParam("item.directoryId", directoryId)
                .SetQueryParam("sort_by", "item.additionalFields.createdDate")
                .SetQueryParam("sort_order", sortOrder)
                .SetQueryParam("size", sizeOfRecordsPerRequest)
                .SetQueryParam("item.locale", "en_US")
                .SetQueryParam("page", page);
        }
    }
}
