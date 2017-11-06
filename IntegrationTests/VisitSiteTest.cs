using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    //TODO: Should those test go into the specific controller test?
    public class VisitSiteTest : IClassFixture<MemoryTestServerClientFixture>
    {
        readonly MemoryTestServerClientFixture _testServerClient;

        public VisitSiteTest(MemoryTestServerClientFixture testServerClient)
        {
            _testServerClient = testServerClient;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/Error")]
        public async Task VisitSiteSuccessfulTest(string site)
        {
            await WhenVisitSite(site);
            ThenSuccess();
        }

		[Theory]
		[InlineData("/Account/Register")]
		[InlineData("/Account/DisplayAll")]
        public async Task VisitSiteThenAccessDeniedTest(string site)
        {
			await WhenVisitSite(site);
            ThenAccessDenied();
        }

        [Theory]
        [InlineData("/Account/Register")]
        [InlineData("/Account/DisplayAll")]
        public async Task VisitSiteAsAdminSuccessfulTest(string site)
		{
            GivenLogedInAsAdmin();
			await WhenVisitSite(site);
			ThenSuccess();
		}

        [Fact]
        public async Task VisitUserSiteWhenNotLogedInThenAccessDeniedTest()
        {
            await WhenVisitUserSite();
			ThenAccessDenied();
        }

		[Fact]
		public async Task VisitUserSiteWhenLogedInThenSuccessfulTest()
		{
            GivenLogedInAsUser();
			await WhenVisitUserSite();
			ThenSuccess();
		}

        [Fact]
        public async Task VisitUserSiteWhenLogedInAsAdminTest()
        {
            GivenLogedInAsAdmin();
            await WhenVisitUserSite();
            ThenSuccess();
        }

		HttpResponseMessage Response;

        void GivenLogedInAsAdmin()
        {
            _testServerClient.AccountHelper.LoginUserAsAdmin(_testServerClient.Admin);
        }

        void GivenLogedInAsUser()
        {
            _testServerClient.AccountHelper.LoginUser(_testServerClient.User);
        }

        async Task WhenVisitSite(string site)
        {
            Response = await _testServerClient.Client.GetAsync(site);
        }

        Task WhenVisitUserSite()
        {
            return WhenVisitSite($"traveller/{_testServerClient.User.UserName}");
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
