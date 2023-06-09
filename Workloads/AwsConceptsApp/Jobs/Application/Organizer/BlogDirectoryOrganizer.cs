using Application.Model;
using System.Text.RegularExpressions;

namespace Organizer
{
    public static class BlogDirectoryOrganizer
    {
        public static void CreateBlogFolders(List<Blog> blogs, string rootFolderPath)
        {
            List<BlogDirectory> directories = CategorizeBlogs(blogs);
            string rootFolderName = "awsblogs";
            string baseFolderPath = Path.Combine(rootFolderPath, rootFolderName);

            // Create the root folder
            Directory.CreateDirectory(baseFolderPath);

            // Sort the directories by year in descending order
            var sortedDirectories = directories.OrderByDescending(d => Convert.ToInt32(d.Name));

            // Assign positions to the year directories
            int yearPosition = 1;
            foreach (var yearDirectory in sortedDirectories)
            {
                string yearFolderPath = SetupYear(baseFolderPath, yearDirectory.Name, yearPosition);

                // Sort the month directories by numerical representation in descending order
                var sortedMonthDirectories = yearDirectory.Subdirectories.OrderByDescending(d => GetMonthNumber(d.Name));

                // Assign positions to the month directories
                int monthPosition = 1;
                foreach (var monthDirectory in sortedMonthDirectories)
                {
                    string monthFolderPath = SetupMonth(yearFolderPath, monthDirectory.Name, monthPosition);

                    SetupWeeks(monthDirectory, monthFolderPath);

                    monthPosition++;
                }

                yearPosition++;
            }
        }

        private static List<BlogDirectory> CategorizeBlogs(List<Blog> blogs)
        {
            var directories = new List<BlogDirectory>();

            foreach (var blog in blogs)
            {
                var year = blog.CreatedDate.Year.ToString();
                var month = blog.CreatedDate.ToString("MMMM");
                var week = $"Week-{GetWeekOfMonth(blog.CreatedDate)}";

                var parentDirectory = directories.FirstOrDefault(d => d.Name == year);
                if (parentDirectory == null)
                {
                    parentDirectory = new BlogDirectory
                    {
                        Name = year,
                        Subdirectories = new List<BlogDirectory>(),
                        Blogs = new List<Blog>(),
                        TotalBlogs = 0
                    };
                    directories.Add(parentDirectory);
                }

                var monthDirectory = parentDirectory.Subdirectories.FirstOrDefault(d => d.Name == month);
                if (monthDirectory == null)
                {
                    monthDirectory = new BlogDirectory
                    {
                        Name = month,
                        Subdirectories = new List<BlogDirectory>(),
                        Blogs = new List<Blog>(),
                        TotalBlogs = 0
                    };
                    parentDirectory.Subdirectories.Add(monthDirectory);
                }

                var weekDirectory = monthDirectory.Subdirectories.FirstOrDefault(d => d.Name == week);
                if (weekDirectory == null)
                {
                    weekDirectory = new BlogDirectory
                    {
                        Name = week,
                        Subdirectories = new List<BlogDirectory>(),
                        Blogs = new List<Blog>(),
                        TotalBlogs = 0
                    };
                    monthDirectory.Subdirectories.Add(weekDirectory);
                }

                weekDirectory.Blogs.Add(blog);
                weekDirectory.TotalBlogs++;
                monthDirectory.TotalBlogs++;
                parentDirectory.TotalBlogs++;
            }

            return directories;
        }

        private static int GetWeekOfMonth(DateTime date)
        {
            // Calculate the week of the month using the ISO 8601 definition
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var weekNumber = (int)Math.Ceiling((date.Day + (int)firstDayOfMonth.DayOfWeek - 1) / 7.0);
            return weekNumber;
        }

        private static string SetupYear(string baseFolderPath, string year, int position)
        {
            string yearFolderPath = Path.Combine(baseFolderPath, year);
            Directory.CreateDirectory(yearFolderPath);
            CreateCategoryFile(year, yearFolderPath, position);
            return yearFolderPath;
        }

        private static string SetupMonth(string yearFolderPath, string month, int position)
        {
            string monthFolderPath = Path.Combine(yearFolderPath, month);
            Directory.CreateDirectory(monthFolderPath);
            CreateCategoryFile(month, monthFolderPath, position);
            return monthFolderPath;
        }

        private static void SetupWeeks(BlogDirectory monthDirectory, string monthFolderPath)
        {
            foreach (var weekDirectory in monthDirectory.Subdirectories)
            {
                string weekFolderPath = Path.Combine(monthFolderPath, weekDirectory.Name);
                Directory.CreateDirectory(weekFolderPath);
                string mdxFilePath = Path.Combine(weekFolderPath, "index.mdx");
                string mdxContent = GenerateWeeklyMdxContent(weekDirectory);
                File.WriteAllText(mdxFilePath, mdxContent);

                //for (int i = 0; i < weekDirectory.Blogs.Count; i++)
                //{
                //    Blog? blog = weekDirectory.Blogs[i];
                //    TimeSpan difference = blog?.CreatedDate != null ? DateTime.Now.Subtract(blog.CreatedDate) : TimeSpan.MaxValue;

                //    // check if the blog is not null and not older than 2 years
                //    if (blog != null && difference.TotalDays <= 2 * 365)
                //    {
                //        var blogJsx = GeneratePerBlogMdxContent(blog);
                //        File.WriteAllText(Path.Combine(weekFolderPath, $"blog{i}.mdx"), blogJsx);
                //    }

                //};

            }
        }

        private static string GenerateWeeklyMdxContent(BlogDirectory weekDirectory)
        {
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("---");
            contentBuilder.AppendLine($"slug: {SanitizeMdxValue(weekDirectory.Name)}-Blogs");
            contentBuilder.AppendLine($"title: {SanitizeMdxValue(weekDirectory.Name)}-Blogs");
            contentBuilder.AppendLine("---");
            contentBuilder.AppendLine("<div class=\"all-blog-posts\">");

            foreach (var blog in weekDirectory.Blogs)
            {
                string sanitizedTitle = SanitizeMdxValue(blog.Title);
                string sanitizedExcerpt = SanitizeMdxValue(blog.PostExcerpt);
                string sanitizedAuthor = string.Join(", ", blog.Author.Select(SanitizeMdxValue));
                contentBuilder.AppendLine("  <div class=\"blog-post\">");
                contentBuilder.AppendLine("    <div class=\"top-container\">");
                contentBuilder.AppendLine($"      <div class=\"image-container\">");
                contentBuilder.AppendLine($"          <img src=\"{SanitizeMdxValue(blog.FeaturedImageUrl)}\" alt=\"{sanitizedTitle}\" class=\"image\"/>");
                contentBuilder.AppendLine("      </div>");
                contentBuilder.AppendLine("      <div class=\"content\">");
                contentBuilder.AppendLine($"          <h3><a href=\"{SanitizeMdxValue(blog.Link)}\">{sanitizedTitle}</a></h3>");
                contentBuilder.AppendLine($"          <p>{sanitizedExcerpt}</p>");
                contentBuilder.AppendLine("          <div class=\"meta-data\">");
                contentBuilder.AppendLine($"            <span>Author: <span class=\"primary-text\">{sanitizedAuthor}</span></span>");
                contentBuilder.AppendLine($"            <span>Created: <span class=\"primary-text\">{blog.DateCreated.ToShortDateString()}</span></span>");
                contentBuilder.AppendLine("          </div>");
                contentBuilder.AppendLine("      </div>");
                contentBuilder.AppendLine("    </div>");
                contentBuilder.AppendLine("  </div>");
            }

            contentBuilder.AppendLine("</div>");

            return contentBuilder.ToString();
        }

        private static string GeneratePerBlogMdxContent(Blog blog)
        {
            string sanitizedTitle = SanitizeTitle(blog.Title);
            string sanitizedSlug = SanitizeSlug(blog.Title);
            string sanitizedExcerpt = SanitizeMdxValue(blog.PostExcerpt);
            string sanitizedAuthor = string.Join(", ", blog.Author.Select(SanitizeMdxValue));

            var blogTagList = blog.Tags.Select(tag => $"{SanitizeTag(tag.Name)}");
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("---");
            contentBuilder.AppendLine($"slug: {sanitizedSlug}");
            contentBuilder.AppendLine($"title: {sanitizedTitle}");
            contentBuilder.AppendLine($"tags: [{string.Join(", ", blogTagList.Select(tag => $"{tag}"))}]");
            contentBuilder.AppendLine("---");

            contentBuilder.AppendLine("  <div class=\"blog-post\">");
            contentBuilder.AppendLine("    <div class=\"top-container\">");
            contentBuilder.AppendLine($"      <div class=\"image-container\">");
            contentBuilder.AppendLine($"          <img src=\"{SanitizeMdxValue(blog.FeaturedImageUrl)}\" alt=\"{sanitizedTitle}\" class=\"image\"/>");
            contentBuilder.AppendLine("      </div>");
            contentBuilder.AppendLine("      <div class=\"content\">");
            contentBuilder.AppendLine($"          <h3><a href=\"{SanitizeMdxValue(blog.Link)}\">{sanitizedTitle}</a></h3>");
            contentBuilder.AppendLine($"          <p>{sanitizedExcerpt}</p>");
            contentBuilder.AppendLine("          <div class=\"meta-data\">");
            contentBuilder.AppendLine($"            <span>Author: <span class=\"primary-text\">{sanitizedAuthor}</span></span>");
            contentBuilder.AppendLine($"            <span>Created: <span class=\"primary-text\">{blog.DateCreated.ToShortDateString()}</span></span>");
            contentBuilder.AppendLine("          </div>");
            contentBuilder.AppendLine("      </div>");
            contentBuilder.AppendLine("    </div>");
            contentBuilder.AppendLine("    <div class=\"tags\">");
            contentBuilder.AppendLine("      <span>Tags: </span>");
            contentBuilder.AppendLine($"      <span class=\"primary-text\">c</span>");
            contentBuilder.AppendLine("    </div>");
            contentBuilder.AppendLine("  </div>");


            return contentBuilder.ToString();
        }
        static string SanitizeTitle(string value)
        {
            return Regex.Replace(SanitizeMdxValue(value), "[^a-zA-Z0-9 ]", "");
        }

        static string SanitizeSlug(string value)
        {
            return SanitizeTitle(value).Replace(" ", "-");
        }
        private static string SanitizeMdxValue(string value)
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
        private static string EscapeJsxBreakingCharacters(string text)
        {
            // Remove '&#nnn;' sequences
            text = Regex.Replace(text, @"&#\d+;", "");

            // Replace two or more consecutive spaces with a single space
            text = Regex.Replace(text, @"\s{2,}", " ");

            // Remove specified characters
            text = text.Replace("{", "").Replace("}", "").Replace("\"", "").Replace("<", "").Replace(">", "");

            return text;
        }
        private static bool HasHtmlTags(string text)
        {
            return Regex.IsMatch(text, @"<\w+(\s[^>]*)?>.*?</\w+>", RegexOptions.Singleline);
        }

        private static string StripHtmlTags(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            string plainText = Regex.Replace(doc.DocumentNode.InnerText, @"\s+", " ").Trim();
            return plainText;
        }

        private static string HtmlEncode(string text)
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
        private static string SanitizeTag(string tag)
        {
            // Remove special characters from the tag
            string sanitizedTag = tag
                .Replace("&", string.Empty);

            return sanitizedTag;
        }

        private static int GetMonthNumber(string monthName)
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            return dtfi.MonthNames.ToList().FindIndex(m => m.Equals(monthName, StringComparison.OrdinalIgnoreCase)) + 1;
        }

        private static void CreateCategoryFile(string label, string folderPath, int position)
        {
            string categoryFilePath = Path.Combine(folderPath, "_category_.json");
            var categoryData = new
            {
                label,
                position,
                link = new
                {
                    type = "generated-index"
                }
            };
            WriteJsonToFile(categoryFilePath, categoryData);
        }

        private static void WriteJsonToFile(string filePath, object data)
        {
            string jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);
        }
    }
}
