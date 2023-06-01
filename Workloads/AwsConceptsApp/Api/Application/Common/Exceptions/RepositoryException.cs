namespace Application.Common.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message,
            string entity,
            System.Net.HttpStatusCode dbStatusCode, object metadata)
            : base(message)
        {
            Data.Add("Entity", entity);
            Data.Add("responseCode", dbStatusCode);
            Data.Add("Metadata", metadata);
        }
    }
}
