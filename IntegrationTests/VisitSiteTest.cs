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
    public class VisitSiteTest : TestBase<MemoryDbContextWithFakeSignInStartup>
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
        public async Task VisitUserSiteWhenNotLogedInThenAccessDenied()
        {
            await GivenUserAccount();
            await WhenVisitUserSite();
			ThenAccessDenied();
        }

		[Fact]
		public async Task VisitUserSiteWhenLogedInThenSuccessful()
		{
            await GivenUserAccountAndLogedIn();
			await WhenVisitUserSite();
			ThenSuccess();
		}

		HttpResponseMessage Response;

        IdentityUser user = new IdentityUser()
        {
			UserName = "someUser",
			Email = "user@test.com"
        };

        async Task GivenLogedInAsAdmin()
        {
            await CreateUserAccount();

            Client.DefaultRequestHeaders.Add("IntegrationTestLogin", new[] { user.UserName, "Administrator" });
        }

        async Task GivenUserAccount()
        {
            await CreateUserAccount();
        }

        async Task GivenUserAccountAndLogedIn()
        {
            await CreateUserAccount();
            LogInUser();
        }

		async Task CreateUserAccount()
		{
			var userManager = Server.Host.Services.GetService(typeof(UserManager<IdentityUser>)) as UserManager<IdentityUser>;
            await userManager.CreateAsync(user);
		}

        void LogInUser()
        {
            Client.DefaultRequestHeaders.Add("IntegrationTestLogin", user.UserName);
        }

        async Task WhenVisitSite(string site)
        {
            Response = await Client.GetAsync(site);
        }

        Task WhenVisitUserSite()
        {
            return WhenVisitSite($"traveller/{user.UserName}");
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
