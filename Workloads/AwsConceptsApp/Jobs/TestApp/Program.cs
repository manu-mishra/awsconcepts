// See https://aka.ms/new-console-template for more information
using TestApp;

var blogs = await BlogProvider.GetAllBlogs();

var categories = BlogProvider.CategorizeBlogs(blogs);
BlogProvider.CreateBlogFolders(categories);

//Console.ReadLine();
