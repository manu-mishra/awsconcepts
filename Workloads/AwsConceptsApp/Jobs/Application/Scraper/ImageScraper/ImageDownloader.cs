using Application.Model;

namespace Application.Scraper.ImageScraper
{
    public static class ImageDownloader
    {
        public static async Task DownloadImage(string imageUrl, string baseDir)
        {
            try
            {
                var uri = new Uri(imageUrl);
                string localPath = Path.Combine(baseDir, string.Join(Path.DirectorySeparatorChar.ToString(), uri.Segments.Skip(1)));
                localPath = Uri.UnescapeDataString(localPath); // Remove any URL-encoded characters from the path

                string directory = Path.GetDirectoryName(localPath);
                Directory.CreateDirectory(directory); // Ensure the directory exists

                if (!File.Exists(localPath))
                {
                    using var stream = await imageUrl.GetStreamAsync();
                    using var fileStream = File.Create(localPath);
                    await stream.CopyToAsync(fileStream);
                }
            }
            catch (FlurlHttpException)
            {

                Console.WriteLine($"cant download {imageUrl}");
                Console.WriteLine("");
            }
        }

        public static async Task DownloadImageImageCards(List<Blog> blogs, string basePath)
        {
            var random = new Random();
            for (int i = 0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                if (!string.IsNullOrWhiteSpace(blog.FeaturedImageUrl))
                {
#if DEBUG

                    Console.Write($"\r Downloading {i} of {blogs.Count}");
#endif
                    await DownloadImage(blog.FeaturedImageUrl, basePath);
                    await Task.Delay(random.Next(50, 101));
                }

            }
        }

    }
}
