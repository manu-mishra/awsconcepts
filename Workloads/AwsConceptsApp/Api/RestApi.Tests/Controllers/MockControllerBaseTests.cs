namespace RestApi.Tests.Controllers
{
    public class MockControllerBaseTests : AbstractControllerTestBase
    {
        [Fact(DisplayName = "InitializeMediator_ReturnsMediator")]
        public void InitializeMediator_ReturnsMediatorFromRequestServices()
        {
            // Arrange
            var expectedMediator = new Mock<IMediator>();
            var requestServices = new ServiceCollection().AddSingleton(expectedMediator.Object).BuildServiceProvider();
            var httpContext = new DefaultHttpContext { RequestServices = requestServices };
            var controller = new MockTestController { ControllerContext = new ControllerContext { HttpContext = httpContext } };

            // Act
            var mediator = controller.GetMediator();

            // Assert
            Assert.Same(expectedMediator.Object, mediator);
        }

        [Fact(DisplayName = "ValidateNullContinuationToken")]
        public async Task ValidateNullContinuationToken()
        {
            // Act
            var response = await client.GetAsync($"/api/MockTest");
                IEnumerable<string>? headers;
                var hasToken = response.Headers.TryGetValues("X-Continuation-Token", out headers);
            // Assert
            Assert.False(hasToken);
            Assert.Null(headers);
        }
        [Fact(DisplayName = "ValidateContinuationToken")]
        public async Task ValidateContinuationToken()
        {
            // Act
            var response = await client.GetAsync($"/api/mocktest/withtoken");
            IEnumerable<string>? headers;
            var hasToken = response.Headers.TryGetValues("X-Continuation-Token", out headers);
            // Assert
            Assert.True(hasToken);
            Assert.NotNull(headers);
            Assert.Equal("Hi", headers.First());
        }
        protected override Mock<IMediator> getMockMediator()
        {
            return new Mock<IMediator>();
        }

        protected override void setupDataForMock()
        {
            // nothing here
        }
    }
}
