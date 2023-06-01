namespace RestApi.Tests.Controllers
{
    public abstract class AbstractControllerTestBase
    {
        protected readonly Mock<IMediator> mockMediator;
        protected readonly HttpClient client;
        public AbstractControllerTestBase()
        {
            setupDataForMock();
            mockMediator = getMockMediator();
            client = GetHttpCLient();
        }

        HttpClient GetHttpCLient()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>()
                .ConfigureServices(ConfigureServices));

            return server.CreateClient();
        }
        void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMediator>(mockMediator.Object);
        }
        protected abstract Mock<IMediator> getMockMediator();
        protected abstract void setupDataForMock();
    }
}
