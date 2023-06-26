using Application.Model;
using Application.Organizer;

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
                var sortedMonthDirectories = yearDirectory.Subdirectories.OrderByDescending(d => Helper.GetMonthNumber(d.Name));

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
                var year = blog.CreatedDate.Year;
                var yearString = year.ToString();
                var month = blog.CreatedDate.ToString("MMMM");
                var week = $"Week-{GetWeekOfMonth(blog.CreatedDate)}";

                var parentDirectory = directories.FirstOrDefault(d => d.Name == yearString);
                if (parentDirectory == null)
                {
                    parentDirectory = new BlogDirectory
                    {
                        Name = yearString,
                        Year = year,
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
                        Year = year,
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
                        Year = year,
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
                string mdxFilePath = Path.Combine(monthFolderPath, $"{weekDirectory.Name}.mdx");
                string mdxContent = BlogContentGenerator.GenerateMdxContent(weekDirectory.Blogs,monthDirectory.Name, monthDirectory.Year, weekDirectory.Name);
                File.WriteAllText(mdxFilePath, mdxContent);
            }
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
            Helper.WriteJsonToFile(categoryFilePath, categoryData);
        }
      
    }
}