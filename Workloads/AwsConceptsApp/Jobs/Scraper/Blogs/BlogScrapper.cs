using Scraper.AwsSearch;

namespace Scraper.Blogs;

internal static class BlogScrapper
{
    public static async Task<List<Blog>> ScrapeAllBlogsListings()
    {
        return ConvertItemsToBlogs(await SearchDirectory.GetSearchResultsAsync("blog-posts", 1000, 500));
    }
    public static async Task FillBlogDetails(Blog blog)
    {
        try
        {
            blog.RawHtml = await blog.Link.GetAsync().ReceiveString();
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Error occurred while fetching the web result for URL: {blog.Link}");
            Console.WriteLine(ex.Message);
        }
    }

    static List<Blog> ConvertItemsToBlogs(List<Record> items)
    {
        var blogs = new List<Blog>();

        foreach (var item in items)
        {
            var blog = ConvertItemToBlog(item.Item);
            blog.Tags = ConvertItemTagToBlogTag(item.Tags);
            blogs.Add(blog);
        }

        return blogs;
    }

    static Blog ConvertItemToBlog(Item item)
    {
        var blog = new Blog
        {
            Id = item.Id,
            Locale = item.Locale,
            DirectoryId = item.DirectoryId,
            Name = item.Name,
            Author = item.Author,
            CreatedBy = item.CreatedBy,
            LastUpdatedBy = item.LastUpdatedBy,
            NumImpressions = item.NumImpressions,
            Score = item.Score,
            DateCreated = item.DateCreated,
            DateUpdated = item.DateUpdated,
            FeaturedImageUrl = item.AdditionalFields.ContainsKey("featuredImageUrl") ? (string)item.AdditionalFields["featuredImageUrl"] : null,
            PostExcerpt = item.AdditionalFields.ContainsKey("postExcerpt") ? (string)item.AdditionalFields["postExcerpt"] : null,
            DisplayDate = item.AdditionalFields.ContainsKey("displayDate") ? (string)item.AdditionalFields["displayDate"] : null,
            Link = item.AdditionalFields.ContainsKey("link") ? (string)item.AdditionalFields["link"] : null,
            CreatedDate = item.AdditionalFields.ContainsKey("createdDate") ? ParseDateTime(item.AdditionalFields["createdDate"]) : default,
            ModifiedDate = item.AdditionalFields.ContainsKey("modifiedDate") ? ParseDateTime(item.AdditionalFields["modifiedDate"]) : default,
            Contributors = item.AdditionalFields.ContainsKey("contributors") ? (string)item.AdditionalFields["contributors"] : null,
            Title = item.AdditionalFields.ContainsKey("title") ? (string)item.AdditionalFields["title"] : null,
            ContentType = item.AdditionalFields.ContainsKey("contentType") ? (string)item.AdditionalFields["contentType"] : null,
            Slug = item.AdditionalFields.ContainsKey("slug") ? (string)item.AdditionalFields["slug"] : null,
        };

        return blog;
    }

    static List<Tag> ConvertItemTagToBlogTag(List<AwsSearch.Tag> tags)
    {
        List<Tag> result = new List<Tag>();
        if (tags != null && tags.Any())
            foreach (AwsSearch.Tag tag in tags)
            {
                if (tag != null && !string.IsNullOrEmpty(tag.Name))
                    result.Add(new Tag { Id = tag.Id, Description = tag.Description, Locale = tag.Locale, Name = tag.Name, TagNamespaceId = tag.TagNamespaceId });
            }

        return result;
    }
    static DateTime ParseDateTime(object obj)
    {
        if (obj is DateTime dateTime)
        {
            return dateTime;
        }

        if (obj is string str)
        {
            return DateTime.Parse(str);
        }

        return default; // Or throw an exception if you prefer
    }

}
