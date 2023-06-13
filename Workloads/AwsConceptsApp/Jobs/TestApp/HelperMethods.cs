using Application.Model;
using Application.Scraper.AwsSearch;
using Application.Scraper.BlogsScraper;
using Newtonsoft.Json;
using Parquet;
using System.IO.Compression;

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
            var allBlogs = await ReadFileContent(filename);
            var allBlogWithDetails = await FillDetails(allBlogs);
            await WriteContentToFile(filename, allBlogWithDetails);
        }

        public static async Task<List<Record>> ReadFileContent(string filename)
        {
            using (var inputFileStream = File.OpenRead(filename))
            using (var streamReader = new StreamReader(inputFileStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var jsonSerializer = new JsonSerializer();
                return await Task.Run(() => jsonSerializer.Deserialize<List<Record>>(jsonReader));
            }
        }


        public static async Task<List<Blog>> FillDetails(List<Record> allBlogs)
        {
            var allBlogWithDetails = await BlogScrapper.ScrapeAllBlogsListings(allBlogs);
            return allBlogWithDetails;
        }

        public static async Task WriteContentToFile(string filename, List<Blog> allBlogWithDetails)
        {
            using (var outputFileStream = File.CreateText($"{filename}Detailed"))
            using (var jsonWriter = new JsonTextWriter(outputFileStream))
            {
                var jsonSerializer = new JsonSerializer();

                await jsonWriter.WriteStartArrayAsync();

                foreach (var blog in allBlogWithDetails)
                {
                    jsonSerializer.Serialize(jsonWriter, blog);
                    await outputFileStream.WriteLineAsync();
                }

                await jsonWriter.WriteEndArrayAsync();
            }
        }

    }
}
