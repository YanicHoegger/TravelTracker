using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using IntegrationTests.Utilities;

namespace IntegrationTests
{
    public class TestServerClientFixture<TStartup> : IDisposable where TStartup : class
    {
        public TestServerClientFixture()
        {
			var builder = new WebHostBuilder()
				.UseContentRoot(ProductionCodePath.GetTravelTracker())
                .UseEnvironment("Development")
				.UseStartup<TStartup>();
            
			Server = new TestServer(builder);
			Client = Server.CreateClient();
        }

        public TestServer Server { get; private set; }

        public HttpClient Client { get; private set; }

        public void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
        }
    }
}
