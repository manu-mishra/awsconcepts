using Application.Scraper.AwsSearch;
using Application.Scraper.BlogsScraper;
using Newtonsoft.Json;

namespace TestApp
{
    internal static class HelperMethods
    {
        public static async Task DownloadAllBlogList()
        {
            var allBlogList = await DirectoryScraper.ScrapeEntireDirectory();
            var allBlogText = JsonConvert.SerializeObject(allBlogList);
            File.WriteAllText($"allBlogList{DateTime.Now.ToString("yyyyMMdd")}", allBlogText);

        }

        public static async Task FillListWithBlogContent(string filename)
        {
            //allBlogList20230613
            var allBlogJson = await File.ReadAllTextAsync(filename);
            var allBlogs = JsonConvert.DeserializeObject<List<Record>>(allBlogJson);
            allBlogJson = string.Empty;
            var allBlogWithDetails = await BlogScrapper.ScrapeAllBlogsListings(allBlogs);
            var allBlogWithDetailsJson = JsonConvert.SerializeObject(allBlogWithDetails);
            File.WriteAllText($"{filename}Detailed", allBlogWithDetailsJson);

        }
    }
}
