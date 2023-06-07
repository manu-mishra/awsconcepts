using Application.Model;
using Application.Scraper.AwsSearch;

namespace Application.Scraper.BlogsScraper;

public static class BlogScrapper
{
    public static async Task<List<Blog>> ScrapeAllBlogsListings(int sizeOfRecordsPerRequest, int maxNumberPages, string sortOrder)
    {
        var blogs = ConvertItemsToBlogs(await SearchDirectory.GetSearchResultsAsync("blog-posts", sizeOfRecordsPerRequest, maxNumberPages, sortOrder));
        if (blogs.Any())
        {
            for (int i = 0; i < blogs.Count; i++)
            {
                var blog = blogs[i];
                await FillBlogDetails(blog);
                System.Diagnostics.Debug.WriteLine($"Fetched blog index {i} for date {blog.CreatedDate}");
                await Task.Delay(TimeSpan.FromMilliseconds(100));
#if DEBUG
                Console.Write($"\rFetched blog index {i} for date {blog.CreatedDate}");
#endif
            }
        }
        return blogs;
    }
    private static async Task FillBlogDetails(Blog blog)
    {
        try
        {
            var response = await blog.Link.GetAsync().ReceiveString();
            ExtractBlogContent(response, blog);
        }
        catch (FlurlHttpException ex)
        {
            blog.RawHtml = "notfound";
            Console.WriteLine($"Error occurred while fetching the web result for URL: {blog.Link}");
            Console.WriteLine(ex.Message);
        }
    }

    private static void ExtractBlogContent(string html, Blog blog)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        HtmlNode articleNode = doc.DocumentNode.SelectSingleNode("//article[@class='blog-post']");
        if (articleNode == null)
            return;
        var postContent= articleNode.SelectSingleNode("//section");
        if (postContent == null)
            return;
        var preTags = postContent.Descendants("pre").ToList();
        foreach (var preTag in preTags){
            preTag.Remove();
        }
        var allImages = postContent.Descendants("img");
        blog.ImagesUsed = new List<string>();
        foreach (var image in allImages)
        {
            // Extract image URL from "src" attribute
            string imageUrl = image.GetAttributeValue("src", "");
            if (!string.IsNullOrEmpty(imageUrl))
            {
                blog.ImagesUsed.Add(imageUrl);
            }
        }
        blog.RawHtml = postContent.InnerHtml;
       
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

    static List<Tag> ConvertItemTagToBlogTag(List<AwsSearch.ItemTag> tags)
    {
        List<Tag> result = new List<Tag>();
        if (tags != null && tags.Any())
            foreach (AwsSearch.ItemTag tag in tags)
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
