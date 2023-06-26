using Application.Model;

namespace Application.Organizer
{
    public class BlogContentGenerator
    {
        public static void SetupBlogs(List<Blog> blogs, string monthFolderPath)
        {
            string mdxFilePath = Path.Combine(monthFolderPath, "index.mdx");
            string mdxContent = GenerateMonthlyMdxContent(blogs);
            File.WriteAllText(mdxFilePath, mdxContent);
        }

        public static string GenerateMonthlyMdxContent(List<Blog> blogs)
        {
            var sb = new StringBuilder();
            sb.Append("---\n");
            sb.Append($"title: \"Blogs of {blogs.First().CreatedDate.ToString("MMMM yyyy")}\"\n");
            sb.Append($"description: \"This is a collection of blogs published in {blogs.First().CreatedDate.ToString("MMMM yyyy")}.\"\n");
            // ...
            return sb.ToString();
        }
    }
}
