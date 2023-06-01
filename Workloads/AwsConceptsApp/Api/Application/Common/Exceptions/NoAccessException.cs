namespace Application.Common.Exceptions
{
    public class NoAccessException : Exception
    {
        public NoAccessException()
            : base()
        {
        }

        public NoAccessException(string message) : base(message)
        {
        }

        public NoAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
