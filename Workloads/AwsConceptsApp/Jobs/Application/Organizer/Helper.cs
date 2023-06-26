using Application.Model;
using System.Text.RegularExpressions;

namespace Application.Organizer
{
    public static class Helper
    {
        public static string SanitizeTitle(string value)
        {
            return Regex.Replace(SanitizeMdxValue(value), "[^a-zA-Z0-9 ]", "");
        }

        public static string SanitizeSlug(string value)
        {
            return SanitizeTitle(value).Replace(" ", "-");
        }
        public static string SanitizeMdxValue(string value)
        {
            if (value == null)
                return string.Empty;

            // Check if the input contains HTML tags
            bool hasHtmlTags = HasHtmlTags(value);

            string sanitizedValue = string.Empty;

            if (hasHtmlTags)
            {
                // Remove HTML tags
                string plainText = StripHtmlTags(value);

                // Encode special characters as HTML entities
                sanitizedValue = HtmlEncode(plainText);
            }
            else
            {
                // Encode special characters as HTML entities directly
                sanitizedValue = HtmlEncode(value);
            }
            sanitizedValue = EscapeJsxBreakingCharacters(sanitizedValue);
            return sanitizedValue;
        }
        public static string EscapeJsxBreakingCharacters(string text)
        {
            // Remove '&#nnn;' sequences
            text = Regex.Replace(text, @"&#\d+;", "");

            // Replace two or more consecutive spaces with a single space
            text = Regex.Replace(text, @"\s{2,}", " ");

            // Remove specified characters
            text = text.Replace("{", "").Replace("}", "").Replace("\"", "").Replace("<", "").Replace(">", "");

            return text;
        }
        public static bool HasHtmlTags(string text)
        {
            return Regex.IsMatch(text, @"<\w+(\s[^>]*)?>.*?</\w+>", RegexOptions.Singleline);
        }

        public static string StripHtmlTags(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            string plainText = Regex.Replace(doc.DocumentNode.InnerText, @"\s+", " ").Trim();
            return plainText;
        }

        public static string HtmlEncode(string text)
        {
            StringBuilder encodedText = new StringBuilder();
            foreach (char c in text)
            {
                if (c > 127)
                {
                    encodedText.AppendFormat("&#{0};", (int)c);
                }
                else
                {
                    encodedText.Append(c);
                }
            }
            return encodedText.ToString();
        }
        public static string SanitizeTag(string tag)
        {
            // Remove special characters from the tag
            string sanitizedTag = tag
                .Replace("&", string.Empty)
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace("\"", string.Empty)
                ;

            return sanitizedTag;
        }

        public static int GetMonthNumber(string monthName)
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            return dtfi.MonthNames.ToList().FindIndex(m => m.Equals(monthName, StringComparison.OrdinalIgnoreCase)) + 1;
        }
        public static void WriteJsonToFile(string filePath, object data)
        {
            string jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);
        }

        private static string GenerateWeeklyMdxContent(BlogDirectory weekDirectory)
        {
            var pageDescription = GetWeekDescription(weekDirectory);
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("---");
            contentBuilder.AppendLine($"slug: {Helper.SanitizeMdxValue(weekDirectory.Name)}-Blogs");
            contentBuilder.AppendLine($"title: {Helper.SanitizeMdxValue(weekDirectory.Name)}-Blogs");
            contentBuilder.AppendLine($"description: {pageDescription}");
            if (weekDirectory.Year >= 2022)
                contentBuilder.AppendLine($"image: TwitterCard.webp");
            contentBuilder.AppendLine($"tags: [{string.Join(", ", GetTags(weekDirectory).Select(tag => $"{tag}"))}]");
            contentBuilder.AppendLine("---");

            contentBuilder.AppendLine("<div class=\"all-blog-posts\">");

            foreach (var blog in weekDirectory.Blogs)
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
                //contentBuilder.AppendLine("    <div class=\"tags\">");
                //contentBuilder.AppendLine("      <span>Tags: </span>");
                //contentBuilder.AppendLine($"      <span class=\"primary-text\">{getTagAnchorsTags(blog)}</span>");
                //contentBuilder.AppendLine("    </div>");
                contentBuilder.AppendLine("  </div>");
            }

            contentBuilder.AppendLine("</div>");

            return contentBuilder.ToString();
        }
        private static string GetWeekDescription(BlogDirectory weekDirectory)
        {
            string response = $"All official AWS blogs created in {weekDirectory.Name}. Chechout full list of historical blogs at www.awsconcepts.com";
            var refBlog = weekDirectory.Blogs.FirstOrDefault();
            if (refBlog != null)
            {
                response = $"All official AWS blogs created in {refBlog.CreatedDate.Year}-{refBlog.CreatedDate.ToString("MMMM")}-{weekDirectory.Name}. Chechout full list of historical blogs at www.awsconcepts.com";
            }
            return response;
        }

        private static string getAuthorAnchorsTags(Blog blog)
        {

            StringBuilder contentBuilder = new StringBuilder();
            foreach (var author in blog.Author)
            {
                foreach (var item in author.Split(","))
                {
                    var authorName = Helper.SanitizeTag(Helper.SanitizeMdxValue(item));
                    contentBuilder.AppendLine($"<a href=\"../../../tags/{authorName.Replace(" ", "-")}\">{authorName}</a>");
                }

            }
            return contentBuilder.ToString();
        }
        private static string getTagAnchorsTags(Blog blog)
        {

            StringBuilder contentBuilder = new StringBuilder();
            foreach (var tag in blog.Tags)
            {

                var tagItem = Helper.SanitizeTag(Helper.SanitizeMdxValue(tag.Name));
                contentBuilder.AppendLine($"<a href=\"../../../tags/{tagItem.Replace(" ", "-")}\">{tagItem}</a>");
            }
            return contentBuilder.ToString();
        }
        private static HashSet<string> GetTags(BlogDirectory weekDirectory)
        {
            HashSet<string> tags = new HashSet<string>();

            foreach (var blog in weekDirectory.Blogs)
            {
                //foreach (var tag in blog.Tags)
                //{
                //    tags.Add(SanitizeTag(tag.Name));
                //}
                foreach (var author in blog.Author)
                {
                    tags.Add(Helper.SanitizeTag(author));
                }
            }

            return tags;
        }

    }
}
