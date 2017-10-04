using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class HomeControllerTest
    {
        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/Error")]
        public async Task VisitSiteSuccessful(string site)
        {
            GivenServerAndClient();
            await WhenVisitSite(site);
            ThenSuccess();
        }

		[Theory]
		[InlineData("/Account/Register")]
		[InlineData("/Account/DisplayAll")]
        public async Task VisitSiteThenAccessDenied(string site)
        {
			GivenServerAndClient();
			await WhenVisitSite(site);
            ThenAccessDenied();
        }

        TestServerClient<MemoryDbContextStartUp> ServerClient;
        HttpResponseMessage Response;

        void GivenServerAndClient()
        {
            ServerClient = new TestServerClient<MemoryDbContextStartUp>();
        }

        async Task WhenVisitSite(string site)
        {
            Response = await ServerClient.Client.GetAsync(site);
        }

        void ThenSuccess()
        {
            //Throws Exception if not success
            Response.EnsureSuccessStatusCode();
        }

        void ThenAccessDenied()
        {
			//TODO: Check right status code
			Assert.Equal(HttpStatusCode.Forbidden, Response.StatusCode);
        }
    }
}
