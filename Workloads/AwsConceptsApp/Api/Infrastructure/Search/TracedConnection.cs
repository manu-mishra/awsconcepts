using Amazon.XRay.Recorder.Handlers.System.Net;
using OpenSearch.Net;

namespace Infrastructure.Search
{
    public class TracedConnection : HttpConnection
    {
        protected override HttpMessageHandler CreateHttpClientHandler(RequestData requestData)
        {
            return new HttpClientXRayTracingHandler(base.CreateHttpClientHandler(requestData));
        }
    }
}
