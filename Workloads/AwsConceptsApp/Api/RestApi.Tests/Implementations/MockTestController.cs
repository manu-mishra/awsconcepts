namespace RestApi.Tests.Implementations
{
    [Route("api/MockTest")]
    [ApiController]
    public class MockTestController:ApiControllerBase
    {
        public IMediator GetMediator()
        {
            return Mediator;
        }

        [HttpGet]
        public List<object> Get(CancellationToken cancellationToken)
        {
            SetContinuationTokens(default);
            return new List<object>();
        }
        [HttpGet("WithToken")]
        public List<object> GetWithToken(CancellationToken cancellationToken)
        {
            SetContinuationTokens("Hi");
            return new List<object>();
        }
    }
}
