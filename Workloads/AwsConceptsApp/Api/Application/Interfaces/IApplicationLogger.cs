namespace Application.Interfaces
{
    public interface IApplicationLogger
    {
        void AddAnnotation(string key, object value);
        void AddMetadata(string key, object value);
        void AddException(Exception ex);
    }
}
