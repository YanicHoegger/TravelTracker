using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IntegrationTests.TestStartups;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests
{
    //TODO: Should those test go into the specific controller test?
    public class VisitSiteTest : TestBase<MemoryDbContextStartUp>
    {
        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/Error")]
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

        [Theory]
        [InlineData("/Account/Register")]
        [InlineData("/Account/DisplayAll")]
        public async Task VisitSiteAsAdminSuccessful(string site)
		{
            await GivenLogedInAsAdmin();
			await WhenVisitSite(site);
			ThenSuccess();
		}

        [Fact]
        public async Task VisitTravellerMaxSiteWhenNotLogedInThenAccessDenied()
        {
            await GivenAccountMax();
			await WhenVisitMaxSite();
			ThenAccessDenied();
        }

		[Fact]
		public async Task VisitTravellerMaxSiteWhenLogedInThenSuccessful()
		{
            await GivenAccountMaxAndLogedIn();
			await WhenVisitMaxSite();
			ThenSuccess();
		}

		HttpResponseMessage Response;

        IdentityUser max = new IdentityUser()
        {
			UserName = "max",
			Email = "max@test.com"
        };
        string password = "ValidPassword123";

        async Task GivenLogedInAsAdmin()
        {
            await CreateAccountMax();

            var userManager = Server.Host.Services.GetService(typeof(UserManager<IdentityUser>)) as UserManager<IdentityUser>;
            await userManager.AddClaimAsync(max, new Claim(ClaimTypes.Role, "Administrator"));

            LogInMax();
        }

        async Task GivenAccountMax()
        {
            await CreateAccountMax();
        }

        async Task GivenAccountMaxAndLogedIn()
        {
            await CreateAccountMax();
            LogInMax();
        }

		async Task CreateAccountMax()
		{
			var userManager = Server.Host.Services.GetService(typeof(UserManager<IdentityUser>)) as UserManager<IdentityUser>;
			await userManager.CreateAsync(max, password);
		}

        void LogInMax()
        {
            Client.DefaultRequestHeaders.Add("IntegrationTestLogin", "max");
        }

        async Task WhenVisitSite(string site)
        {
            Response = await Client.GetAsync(site);
        }

        Task WhenVisitMaxSite()
        {
            return WhenVisitSite("traveller/max");
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
