using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;

namespace IntegrationTests.Utilities
{
    public class AccountHelper
    {
        public UserManager<IdentityUser> UserManager { get; private set; }
        public SignInManager<IdentityUser> SignInManager { get; private set; }
        
        public AccountHelper(TestServer server)
        {
            UserManager = (UserManager<IdentityUser>)server.Host.Services.GetService(typeof(UserManager<IdentityUser>));
            SignInManager = (SignInManager<IdentityUser>)server.Host.Services.GetService(typeof(SignInManager<IdentityUser>));
        }

		public async Task CreateUserAsync(IdentityUser user, string password)
		{
			await UserManager.CreateAsync(user, password);
		}

		public async Task LoginUserAsync(IdentityUser user, string password)
		{
			await SignInManager.PasswordSignInAsync(user, password, false, false);
		}

		public async Task CreateAndLoginUser(IdentityUser user, string password)
		{
			await CreateUserAsync(user, password);
			await LoginUserAsync(user, password);
		}
    }
}
