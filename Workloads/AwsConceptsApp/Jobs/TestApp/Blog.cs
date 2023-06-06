namespace TestApp
{
    public class Blog
    {
        public string Id { get; set; }

        public string Locale { get; set; }

        public string DirectoryId { get; set; }

        public string Name { get; set; }

        public List<string> Author { get; set; }

        public List<string> CreatedBy { get; set; }

        public List<string> LastUpdatedBy { get; set; }

        public int NumImpressions { get; set; }

        public double Score { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string PostExcerpt { get; set; }

        public DateTime CreatedDate { get; set; }

        public string DisplayDate { get; set; }

        public string Link { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Contributors { get; set; }

        public string Title { get; set; }

        public string ContentType { get; set; }

        public string Slug { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
