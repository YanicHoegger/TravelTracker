using System.Threading.Tasks;
using IntegrationTests.TestStartups;
using IntegrationTests.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IntegrationTests.AccountControllerTests
{
    public class AccountControllerTestsBase : TestBase<SqlliteDbContextWithFakeSignInStartup>
    {
        public AccountControllerTestsBase()
        {
            CreateUserAndLogIn().Wait();
        }

        private async Task CreateUserAndLogIn()
        {
            var user = new IdentityUser()
            {
                UserName = "test",
                Email = "test@test.com"
            };

            var userManager = (UserManager<IdentityUser>)Server.Host.Services.GetService(typeof(UserManager<IdentityUser>));
            await userManager.CreateAsync(user);

            Client.DefaultRequestHeaders.Add("IntegrationTestLogin", new[] { user.UserName, "Administrator" });
        }

        protected async Task<string> GetAntiForgeryToken(string action)
        {
            var response = await CookieClient.GetAsync($"/Account/{action}");
            response.EnsureSuccessStatusCode();

            return await AntiForgeryHelper.ExtractAntiForgeryToken(response);
        }
    }
}
