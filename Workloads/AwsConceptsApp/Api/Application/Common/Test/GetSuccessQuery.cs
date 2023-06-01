using System.Diagnostics;

namespace Application.Common.Test
{
    public class GetSuccessQuery : IRequest<string>
    {
        public GetSuccessQuery(string message)
        {
            this.Message = message;
        }
        public string Message { get; }
    }
    public class GetSuccessQueryHandler : IRequestHandler<GetSuccessQuery, string>
    {
        public GetSuccessQueryHandler()
        {
        }
        Task<string> IRequestHandler<GetSuccessQuery, string>.Handle(GetSuccessQuery request, CancellationToken cancellationToken)
        {
            string? traceId = Activity.Current?.TraceId.ToHexString();
            string version = "1";
            string? epoch = traceId?.Substring(0, 8);
            string? random = traceId?.Substring(8);
            string response = "{" + "\"traceId\"" + ": " + "\"" + version + "-" + epoch + "-" + random + "\"" + "}";

            return Task.FromResult($"I got {response}");
        }
    }
}
