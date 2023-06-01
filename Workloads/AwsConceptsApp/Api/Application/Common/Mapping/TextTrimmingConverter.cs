namespace Application.Common.Mapping
{
    public class TextTrimmingConverter : IValueConverter<string, string>
    {
        public string Convert(string source, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source) && source.Length > 100)
                return source.Substring(0, 100) + "...";
            return source;
        }
    }
}
