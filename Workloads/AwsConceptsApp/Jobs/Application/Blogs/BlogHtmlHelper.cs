using Application.Model;
using System.Text.RegularExpressions;

namespace Application.Blogs
{
    public static class BlogHtmlHelper
    {
        public static List<string> ExtractHtmlTextLines(this Blog blog)
        {
            List<string> response = new List<string>();

            if (blog != null && !string.IsNullOrWhiteSpace(blog.RawHtml) && !string.Equals(blog.RawHtml, "notfound", StringComparison.OrdinalIgnoreCase))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(blog.RawHtml);

                HtmlNode articleNode = doc.DocumentNode.SelectSingleNode("//article[@class='blog-post']");
                if (articleNode != null)
                {
                    var postContent = articleNode.SelectSingleNode("//section");

                    // Extract all text without HTML tags and attributes.
                    string textWithoutTags = HtmlEntity.DeEntitize(postContent.InnerHtml);
                    textWithoutTags = Regex.Replace(textWithoutTags, "<.*?>", " ");

                    // Break the text into individual lines.
                    string[] lines = textWithoutTags.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines)
                    {
                        // Remove any special characters, empty spaces, and trim the line.
                        string cleanedLine = Regex.Replace(line, @"[^\w\s\.]", "").Trim();

                        // Check if the line has a full stop or end of line punctuation.
                        if (cleanedLine.EndsWith(".") || cleanedLine.EndsWith("!") || cleanedLine.EndsWith("?") || string.IsNullOrEmpty(cleanedLine))
                        {
                            // Add the cleaned line to the response list.
                            response.Add(cleanedLine);
                        }
                        else
                        {
                            // Combine the line with the next line to form a complete sentence.
                            string previousLine = response.LastOrDefault();
                            string combinedLine = (previousLine + " " + cleanedLine).Trim();

                            // Update the last item in the response list with the combined line.
                            response[response.Count - 1] = combinedLine;
                        }
                    }
                }
            }

            return response;
        }

    }
}
