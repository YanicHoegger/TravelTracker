using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class VisitSiteTest : MemoryDbTestBase
    {
        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/AccessDenied")]
        public async Task VisitSiteSuccessful(string site)
        {
            await WhenVisitSite(site);
            ThenSuccess();
        }

		[Theory]
		[InlineData("/Account/Register")]
		[InlineData("/Account/DisplayAll")]
        public async Task VisitSiteThenAccessDenied(string site)
        {
			await WhenVisitSite(site);
            ThenAccessDenied();
        }

        HttpResponseMessage Response;

        public VisitSiteTest(TestServerClientFixture<MemoryDbContextStartUp> testServerClient) : base(testServerClient)
        {
        }

        async Task WhenVisitSite(string site)
        {
            Response = await Client.GetAsync(site);
        }

        void ThenSuccess()
        {
            //Throws Exception if not success
            Response.EnsureSuccessStatusCode();
        }

        void ThenAccessDenied()
        {
			//TODO: Check right status code
            Assert.Equal(HttpStatusCode.Unauthorized, Response.StatusCode);
        }
    }
}
