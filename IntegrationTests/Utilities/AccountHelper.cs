using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;

namespace IntegrationTests.Utilities
{
    public class AccountHelper
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly HttpClient _client;
        
        public AccountHelper(TestServer server, HttpClient client)
        {
            _userManager = (UserManager<IdentityUser>)server.Host.Services.GetService(typeof(UserManager<IdentityUser>));
            _client = client;
        }

		public async Task CreateUserAsync(IdentityUser user, string password)
		{
			await _userManager.CreateAsync(user, password);
		}

        public async Task CreateUserAsync(IdentityUser user)
        {
            await _userManager.CreateAsync(user);
        }

        public void LoginUser(IdentityUser user)
        {
            _client.DefaultRequestHeaders.Remove("IntegrationTestLogin");

            _client.DefaultRequestHeaders.Add(
                "IntegrationTestLogin", user.UserName);
        }

        public void LoginUserAsAdmin(IdentityUser user)
        {
            _client.DefaultRequestHeaders.Remove("IntegrationTestLogin");

            _client.DefaultRequestHeaders.Add(
                "IntegrationTestLogin", 
                new[] { user.UserName, "Administrator" });
        }
    }
}
