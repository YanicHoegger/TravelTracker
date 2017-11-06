using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IntegrationTests.TestStartups;

namespace IntegrationTests
{
    public class MemoryTestServerClientFixture : TestBase<MemoryDbContextStartUp>
    {
        public MemoryTestServerClientFixture()
        {
            Admin = new IdentityUser()
            {
                UserName = "admin",
                Email = "admin@test.com"
            };
            AdminPassword = "AAss11";

            User = new IdentityUser()
            {
                UserName = "someUser",
                Email = "user@test.com"
            };
            UserPassword = "BBbb22";

            var userManager = (UserManager<IdentityUser>)Server.Host.Services.GetService(typeof(UserManager<IdentityUser>));

            userManager.CreateAsync(Admin, AdminPassword).Wait();
            userManager.CreateAsync(User, UserPassword).Wait();
        }

        public IdentityUser Admin { get; }
        public string AdminPassword { get; }
        public IdentityUser User { get; }
        public string UserPassword { get; }
    }
}
