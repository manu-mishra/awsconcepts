using Application.Model;
using Application.Scraper.AwsSearch;
using System.Diagnostics;

namespace Application.Scraper.BlogsScraper;

public static class BlogScrapper
{
    public static async Task<List<Blog>> ScrapeAllBlogsListings(List<Record> blogsItems)
    {
        var blogs = ConvertItemsToBlogs(blogsItems);
        if (blogs.Any())
        {
            for (int i = 0; i < blogs.Count; i++)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(50));
                var blog = blogs[i];
                await FillBlogDetails(blog);
                Debug.WriteLine($"Fetched blog index {i} for date {blog.CreatedDate}");
            }
        }
        return blogs;
    }

    private static async Task FillBlogDetails(Blog blog)
    {
        try
        {
            var response = await blog.Link.GetAsync().ReceiveString();
            blog.RawHtml = response;
            ExtractImagesUsed(blog);
        }
        catch (FlurlHttpException ex)
        {
            blog.RawHtml = "notfound";
            Debug.WriteLine($"Error occurred while fetching the web result for URL: {blog.Link}");
            Debug.WriteLine(ex.Message);
        }
    }

    private static void ExtractImagesUsed(Blog blog)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(blog.RawHtml);

        HtmlNode articleNode = doc.DocumentNode.SelectSingleNode("//article[@class='blog-post']");
        if (articleNode == null)
            return;

        // Extract images from article content
        var postContent = articleNode.SelectSingleNode("//section");
        ExtractImagesFromHtmlNode(blog, postContent);

        // Extract images from header meta tags
        var metaTags = doc.DocumentNode.SelectNodes("//meta");
        if (metaTags != null)
        {
            foreach (var metaTag in metaTags)
            {
                string imageUrl = null;
                if (metaTag.Attributes["property"]?.Value.StartsWith("og:image") == true)
                {
                    imageUrl = metaTag.Attributes["content"]?.Value;
                }
                else if (metaTag.Attributes["name"]?.Value.StartsWith("twitter:image") == true)
                {
                    imageUrl = metaTag.Attributes["content"]?.Value;
                }

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    blog.ImagesUsed.Add(imageUrl);
                }
            }
        }
    }

    private static void ExtractImagesFromHtmlNode(Blog blog, HtmlNode htmlNode)
    {
        blog.ImagesUsed = new List<string>();
        var allImages = htmlNode.Descendants("img");
        foreach (var image in allImages)
        {
            // extract image url from "src" attribute
            string imageUrl = image.GetAttributeValue("src", "");
            if (!string.IsNullOrEmpty(imageUrl))
            {
                blog.ImagesUsed.Add(imageUrl);
            }
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
