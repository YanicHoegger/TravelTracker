using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace IntegrationTests
{
    public class TestServerClient<TStartup> where TStartup : class
    {
        public TestServerClient()
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
    }
}
