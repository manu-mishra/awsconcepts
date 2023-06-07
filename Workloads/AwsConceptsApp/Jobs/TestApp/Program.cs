// See https://aka.ms/new-console-template for more information
using Application.Model;
using Application.Scraper.BlogsScraper;
using Newtonsoft.Json;
using Organizer;
using static System.Reflection.Metadata.BlobBuilder;


List<Blog> allBlogs = await ReadAndMergeBlogsAsync("AllBlogs1.json", "AllBlogs2.json");
var allBlogsString = JsonConvert.SerializeObject(allBlogs);
await File.WriteAllTextAsync("allblogs.json", allBlogsString);

//BlogDirectoryOrganizer.CreateBlogFolders(allBlogs, "C:\\Users\\manumishra\\source\\repos\\manu-mishra\\awsconcepts\\Workloads\\AwsConceptsApp\\Data");

//Console.ReadLine();

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

