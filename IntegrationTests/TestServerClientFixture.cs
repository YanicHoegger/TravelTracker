using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace IntegrationTests
{
    public class TestServerClientFixture<TStartup> : IDisposable where TStartup : class
    {
        public TestServerClientFixture()
        {
            //TODO: Find a way to resolve path right
			var builder = new WebHostBuilder()
				.UseContentRoot("/Users/lucyrebecca/RiderProjects/TravelTracker/TravelTracker")
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
