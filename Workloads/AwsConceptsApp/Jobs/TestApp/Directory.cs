namespace TestApp
{
    public class BlogDirectory
    {
        public string Name { get; set; }
        public List<BlogDirectory> Subdirectories { get; set; }
        public List<Blog> Blogs { get; set; }
        public int TotalBlogs { get; set; }
    }
}
