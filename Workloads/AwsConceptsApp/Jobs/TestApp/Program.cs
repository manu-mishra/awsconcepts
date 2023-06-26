// See https://aka.ms/new-console-template for more information
using Application.FileHelpers;
using Application.Model;
using Newtonsoft.Json;
using Organizer;
using TestApp;

// Step 1
// Scrape entire directory
//await HelperMethods.DownloadAllBlogList();

// Step 2
// fetchDetails
//await HelperMethods.FillListWithBlogContent("allBlogList20230613","allBlogList20230613Detailed");


//Step 3
//await HelperMethods.PrepareTextForModelTraining("allBlogList20230613Detailed", "TokenTrainingText.txt");

//Console.ReadLine();


var allBlogs =  (new List<Blog>()).FillFromFile("allBlogList20230613Detailed"); 

BlogDirectoryOrganizer.CreateBlogFolders(allBlogs, "C:\\Users\\manumishra\\source\\repos\\manu-mishra\\awsconcepts\\Workloads\\AwsConceptsApp\\ui\\");

Console.ReadLine();