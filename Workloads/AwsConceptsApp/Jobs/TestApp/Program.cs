// See https://aka.ms/new-console-template for more information
using Application.Model;
using Application.Scraper.BlogsScraper;
using Application.Scraper.ImageScraper;
using Newtonsoft.Json;
using Organizer;


var allBlogs = (await GetAllBlogsData());//.Take(10).ToList();
//await ImageDownloader.DownloadImageImageCards(allBlogs, "images");

BlogDirectoryOrganizer.CreateBlogFolders(allBlogs, "C:\\Users\\manumishra\\source\\repos\\manu-mishra\\awsconcepts\\Workloads\\AwsConceptsApp\\ui\\");

//Console.ReadLine();
static async Task<List<Blog>> GetAllBlogsData()
{
    // Process the first file
    string data1 = await File.ReadAllTextAsync("allblogs.json");
    return JsonConvert.DeserializeObject<List<Blog>>(data1);
}

static async Task<List<Blog>> ReadAndMergeBlogsAsync(string file1Path, string file2Path)
{
    // Create a HashSet to keep track of unique blog ids
    var uniqueIds = new HashSet<string>();
    var allBlogs = new List<Blog>();

    // Process the first file
    string data1 = await File.ReadAllTextAsync(file1Path);
    List<Blog> blogs1 = JsonConvert.DeserializeObject<List<Blog>>(data1);

    foreach (var blog in blogs1)
    {
        if (uniqueIds.Add(blog.Id)) // Add returns false if the item already exists
        {
            allBlogs.Add(blog);
        }
    }

    // Process the second file
    string data2 = await File.ReadAllTextAsync(file2Path);
    List<Blog> blogs2 = JsonConvert.DeserializeObject<List<Blog>>(data2);

    foreach (var blog in blogs2)
    {
        if (uniqueIds.Add(blog.Id)) // Add returns false if the item already exists
        {
            allBlogs.Add(blog);
        }
    }

    return allBlogs;
}

