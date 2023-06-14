using Application.Scraper.AwsSearch;
using Application.Scraper.BlogsScraper;
using Newtonsoft.Json;
using Application.FileHelpers;
using Application.Blogs;
using Application.Model;

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

        public static async Task FillListWithBlogContent(string filename, string targetFileName)
        {
            var allBlogs = (new List<Record>()).FillFromFile(filename);
            var allBlogWithDetails = await BlogScrapper.ScrapeAllBlogsListings(allBlogs);
            await allBlogWithDetails.WriteToJsonFile(targetFileName);
        }
     
        public static async Task PrepareTextForModelTraining(string filename, string targetFileName)
        {
            var allBlogs = (new List<Blog>()).FillFromFile(filename);
            using (StreamWriter writer = new StreamWriter(targetFileName))
            {
                foreach (var item in allBlogs)
                {
                    var lines = item.ExtractHtmlTextLines();

                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            await writer.WriteLineAsync(line);
                        }
                    }
                }
            }

        }

    }
}
