using System.Threading.Tasks;
using IntegrationTests.TestStartups;
using IntegrationTests.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IntegrationTests.AccountControllerTests
{
    public class AccountControllerTestsBase : TestBase<SqlliteDbContextStartup>
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

            await AccountHelper.CreateUserAsync(user);
            AccountHelper.LoginUserAsAdmin(user);
        }

        protected async Task<string> GetAntiForgeryToken(string action)
        {
            var response = await CookieClient.GetAsync($"/Account/{action}");
            response.EnsureSuccessStatusCode();

            return await AntiForgeryHelper.ExtractAntiForgeryToken(response);
        }
    }
}
