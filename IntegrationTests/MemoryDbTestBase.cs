using System;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace IntegrationTests
{
    public abstract class MemoryDbTestBase : IClassFixture<TestServerClientFixture<MemoryDbContextStartUp>>
    {
        readonly TestServerClientFixture<MemoryDbContextStartUp> _testServerClient;

        public MemoryDbTestBase(TestServerClientFixture<MemoryDbContextStartUp> testServerClient)
        {
            _testServerClient = testServerClient;
        }

        public TestServer Server => _testServerClient.Server;

        public HttpClient Client => _testServerClient.Client;
    }
}
