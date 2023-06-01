using System.Diagnostics;

namespace Application.Common.Behaviours
{
    public class RequestTracingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IIdentity user;

        public RequestTracingBehaviour(IIdentity user)//, IApplicationLogger logger)
        {
            this.user = user;
            //this.logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            try
            {
                Activity.Current?.AddTag("user", user.Id);
                Activity.Current?.AddTag("domain", request.GetType().ToString());
                Activity.Current?.AddBaggage("otel-user", user.Id);
                Activity.Current?.AddEvent(new ActivityEvent(request.GetType().ToString()));
                string requestDetails = System.Text.Json.JsonSerializer.Serialize(request);
                Activity.Current?.AddBaggage("request", requestDetails);

                return await next();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                throw;
            }
        }
    }
}
