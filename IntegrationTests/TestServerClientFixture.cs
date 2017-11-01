using System;
using TravelTracker;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace IntegrationTests
{
    public class TestServerClientFixture<TStartup> : TestBase<TStartup> where TStartup : Startup, IDisposable
    {
        public TestServerClientFixture()
        {
            Admin = new IdentityUser()
            {
                UserName = "admin",
                Email = "admin@test.com"
            };

            User = new IdentityUser()
            {
                UserName = "someUser",
                Email = "user@test.com"
            };

            var userManager = (UserManager<IdentityUser>)Server.Host.Services.GetService(typeof(UserManager<IdentityUser>));

            userManager.CreateAsync(Admin).Wait();
            userManager.CreateAsync(User).Wait();
        }

        public IdentityUser Admin { get; }
        public IdentityUser User { get; }
    }
}
