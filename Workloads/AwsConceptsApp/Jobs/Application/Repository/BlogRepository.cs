using Application.Model;

namespace Application.Repository
{
    internal class BlogRepository
    {
        public static async Task<List<Blog>> ReadBlogsFromJsonFile(string filePath = "Blogs.json")
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"File not found for path {filePath}");
            }
            var jsonData = await File.ReadAllTextAsync(filePath);
            var blogs = JsonConvert.DeserializeObject<List<Blog>>(jsonData);
            return blogs;
        }

        public static async Task<List<Blog>> WriteBlogsToJsonFile(List<Blog> blogs,string filePath = "Blogs.json")
        {
            var json = JsonConvert.SerializeObject(blogs, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, json);
            return blogs;
        }
    }
}
