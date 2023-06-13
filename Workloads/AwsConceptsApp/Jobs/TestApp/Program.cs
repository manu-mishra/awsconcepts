// See https://aka.ms/new-console-template for more information
using TestApp;

// Step 1
// Scrape entire directory
//await HelperMethods.DownloadAllBlogList();

// Step 2
// fetchDetails
await HelperMethods.FillListWithBlogContent("allBlogList20230613");


Console.ReadLine();


//var allBlogs = (await GetAllBlogsData());//.Take(10).ToList();
////await ImageDownloader.DownloadImageImageCards(allBlogs, "images");

//BlogDirectoryOrganizer.CreateBlogFolders(allBlogs, "C:\\Users\\manumishra\\source\\repos\\manu-mishra\\awsconcepts\\Workloads\\AwsConceptsApp\\ui\\");

////Console.ReadLine();
//static async Task<List<Blog>> GetAllBlogsData()
//{
//    // Process the first file
//    string data1 = await File.ReadAllTextAsync("allblogs.json");
//    return JsonConvert.DeserializeObject<List<Blog>>(data1);
//}