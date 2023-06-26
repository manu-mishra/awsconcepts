using Application.Model;

namespace Application.Organizer
{
    public class BlogContentGenerator
    {
        public static string GenerateMdxContent(List<Blog> blogs, string month, int year, string week)
        {
            var pageDescription = GetDescription(month, year, week);
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("---");
            contentBuilder.AppendLine($"slug: {Helper.SanitizeMdxValue(week)}-Blogs");
            contentBuilder.AppendLine($"title: {Helper.SanitizeMdxValue(week)}-Blogs");
            contentBuilder.AppendLine($"description: {pageDescription}");
            contentBuilder.AppendLine("---");

            contentBuilder.AppendLine("<div class=\"all-blog-posts\">");

            foreach (var blog in blogs)
            {
                string sanitizedTitle = Helper.SanitizeMdxValue(blog.Title);
                string sanitizedExcerpt = Helper.SanitizeMdxValue(blog.PostExcerpt);
                string sanitizedAuthor = string.Join(", ", blog.Author.Select(x => Helper.SanitizeTag(Helper.SanitizeMdxValue(x))));
                contentBuilder.AppendLine("  <div class=\"blog-post\">");
                contentBuilder.AppendLine("    <div class=\"top-container\">");
                contentBuilder.AppendLine($"      <div class=\"image-container\">");
                contentBuilder.AppendLine($"          <img src=\"{Helper.SanitizeMdxValue(blog.FeaturedImageUrl)}\" alt=\"{sanitizedTitle}\" class=\"image\"/>");
                contentBuilder.AppendLine("      </div>");
                contentBuilder.AppendLine("      <div class=\"content\">");
                contentBuilder.AppendLine($"          <h3><a href=\"{Helper.SanitizeMdxValue(blog.Link)}\">{sanitizedTitle}</a></h3>");
                contentBuilder.AppendLine($"          <p>{sanitizedExcerpt}</p>");
                contentBuilder.AppendLine("          <div class=\"meta-data\">");
                contentBuilder.AppendLine($"            <span class=\"meta-item\">Author: <span class=\"primary-text\">{getAuthorAnchorsTags(blog)}</span></span>");
                contentBuilder.AppendLine($"            <span>Created: <span class=\"primary-text\">{blog.DateCreated.ToShortDateString()}</span></span>");
                contentBuilder.AppendLine("          </div>");
                contentBuilder.AppendLine("      </div>");
                contentBuilder.AppendLine("    </div>");
                contentBuilder.AppendLine("  </div>");
            }

            contentBuilder.AppendLine("</div>");

            return contentBuilder.ToString();
        }

        private static string getAuthorAnchorsTags(Blog blog)
        {
            StringBuilder contentBuilder = new StringBuilder();
            foreach (var author in blog.Author)
            {
                foreach (var item in author.Split(","))
                {
                    var authorName = Helper.SanitizeTag(Helper.SanitizeMdxValue(item));
                    contentBuilder.AppendLine($"{authorName} ");
                }
            }
            return contentBuilder.ToString();
        }

        private static string GetDescription(string month, int year, string week)
        {
            string response = $"All official AWS blogs created in {week} of {month} - {year}. Checkout the full list of historical blogs at www.awsconcepts.com";
            return response;
        }

        private static HashSet<string> GetTags(List<Blog> blogs)
        {
            HashSet<string> tags = new HashSet<string>();
            foreach (var blog in blogs)
            {
                foreach (var author in blog.Author)
                {
                    tags.Add(Helper.SanitizeTag(author));
                }
            }
            return tags;
        }
    }

}
