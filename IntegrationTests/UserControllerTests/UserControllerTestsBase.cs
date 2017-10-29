using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntegrationTests.TestStartups;
using IntegrationTests.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IntegrationTests.UserControllerTests
{
    public class UserControllerTestsBase : TestBase<SqlliteDbContextWithFakeSignInStartup>
    {
        protected UserControllerTestsBase()
        {
            User = new IdentityUser()
            {
                UserName = "test",
                Email = "test@test.com"
            };
            Password = "AAaa11";

            CreateUserAndLogIn().Wait();
        }

        protected IdentityUser User { get; }
        protected string Password { get; }

        private async Task CreateUserAndLogIn()
        {
            var userManager = (UserManager<IdentityUser>)Server.Host.Services.GetService(typeof(UserManager<IdentityUser>));
            await userManager.CreateAsync(User, Password);

            Client.DefaultRequestHeaders.Add("IntegrationTestLogin", User.UserName);
        }

        protected async Task<string> GetAntiForgeryToken()
        {
            var response = await CookieClient.GetAsync($"/traveller/{User.UserName}");
            response.EnsureSuccessStatusCode();

            return await AntiForgeryHelper.ExtractAntiForgeryToken(response);
        }

        protected bool TryGetDbValue(string valueSelector, out string value)
        {
            var success = false;

            using (var connection = Startup.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM AspNetUsers";
                    var dbReader = command.ExecuteReader();

                    if(!dbReader.Read())
                    {
                        throw new Exception("Database is empty");
                    }

                    value = (string)dbReader[valueSelector];
                    success = true;
                }

                connection.Close();
            }

            return success;
        }
    }
}
