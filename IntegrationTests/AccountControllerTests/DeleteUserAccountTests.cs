using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IntegrationTests.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.AccountControllerTests
{
    public class DeleteUserAccountTests : AccountControllerTestsBase
    {
        [Fact]
        public async Task DeleteUserAccountTest()
        {
            await GivenAccount();

            await WhenDeleteAccount();

            await ThenAccountDeleted();
        }

        IdentityUser user;
        HttpResponseMessage response;

        async Task GivenAccount()
        {
            user = new IdentityUser()
            {
                UserName = "ToDeleteUser",
                Email = "something@notimportant.com"
            };

            var userManager = (UserManager<IdentityUser>)Server.Host.Services.GetService(typeof(UserManager<IdentityUser>));
            await userManager.CreateAsync(user);
        }

        async Task WhenDeleteAccount()
        {
            var formData = new Dictionary<string, string>()
            {
                {"__RequestVerificationToken", await GetAntiForgeryToken("DisplayAll")},
                {"username", user.UserName}
            };

            response = await CookieClient.PostAsync("Account/DeleteUser", formData);
        }

        async Task ThenAccountDeleted()
        {
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);

            var redirectLocation = response.Headers.Location;
            var redirectedResponse = await Client.GetAsync("Account/" + redirectLocation.OriginalString);
            redirectedResponse.EnsureSuccessStatusCode();

            using (var connection = Startup.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM AspNetUsers";
                    var dbReader = await command.ExecuteReaderAsync();

                    //The new account was in the secound row. In the first row is the admin account
                    Assert.True(dbReader.Read());
                    Assert.False(dbReader.Read());
                }

                connection.Close();
            }
        }
    }
}
