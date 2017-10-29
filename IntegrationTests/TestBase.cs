using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TravelTracker;
using IntegrationTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Linq;

namespace IntegrationTests
{
    public abstract class TestBase<TStartup> : IDisposable where TStartup : Startup, IDisposable
    {
        public TestBase()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(ProductionCodePath.GetTravelTracker())
                .UseEnvironment("Development")
                .ConfigureServices(services => {

                    Startup = ConfigureStartUpService(services);

            });

			Server = new TestServer(builder);
			Client = Server.CreateClient();
            CookieClient = new CookieClient(Client);
        }

        static TStartup ConfigureStartUpService(IServiceCollection serviceCollection)
        {
            var enviroment = serviceCollection.BuildServiceProvider().GetServices<IHostingEnvironment>().Single();

            var startupType = typeof(TStartup);
            var constructor = startupType.GetConstructor(new Type[]{ typeof(IHostingEnvironment) });

            var startup = (TStartup)constructor.Invoke(new object[] { enviroment });

            serviceCollection.AddSingleton(typeof(IStartup), startup);

            return startup;
        }

        public TestServer Server { get; }

        public HttpClient Client { get; }
        public CookieClient CookieClient { get; }

        public TStartup Startup { get; private set; }

        public virtual void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
            Startup.Dispose();
        }
    }
}
