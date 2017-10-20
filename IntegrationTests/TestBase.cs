using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TravelTracker;
using IntegrationTests.Utilities;

namespace IntegrationTests
{
    public abstract class TestBase<TStartup> : IDisposable where TStartup : Startup
    {
        public TestBase()
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

        public virtual void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
        }

    }
}
