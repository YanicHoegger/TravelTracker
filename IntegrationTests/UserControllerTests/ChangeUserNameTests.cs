using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.UserControllerTests
{
    public class ChangeUserNameTests : UserControllerTestsBase
    {
        [Fact]
        public async Task ChangeUserNameSuccessfulTest()
        {
            await GivenRightNewUserName();

            await WhenChangeUserName();

            await ThenSuccessfulUserNameChanged();
        }

        [Fact]
        public async Task ChangeUserNameNotSuccessfulTest()
        {
            await GivenAlreayUsedNewUserName();

            await WhenChangeUserName();

            await ThenNotSuccessfulUserNameChanged();
        }

        string NewUserName;
        HttpResponseMessage Response;

        async Task GivenRightNewUserName()
        {
            await CreateUserAndLogIn();

            NewUserName = "newName";
        }

        async Task GivenAlreayUsedNewUserName()
        {
            await CreateUserAndLogIn();

            var anOtherUser = new IdentityUser()
            {
                UserName = "alreadyusedName"
            };

            var userManager = (UserManager<IdentityUser>)Server.Host.Services.GetService(typeof(UserManager<IdentityUser>));
            await userManager.CreateAsync(User);

            NewUserName = anOtherUser.UserName;
        }

        async Task WhenChangeUserName()
        {
            var formData = new Dictionary<string, string>
              {
                {"__RequestVerificationToken", await GetAntiForgeryToken()},
                {"NewUserName.NewUserName", NewUserName}
              };

            Response = await CookieClient.PostAsync($"/traveller/{User.UserName}/ChangeUserName", formData);
        }

        async Task ThenSuccessfulUserNameChanged()
        {
            Assert.Equal(HttpStatusCode.Found, Response.StatusCode);

            Client.DefaultRequestHeaders.Remove("IntegrationTestLogin");
            Client.DefaultRequestHeaders.Add("IntegrationTestLogin", NewUserName);

            var redirectLocation = Response.Headers.Location;
            var redirectedResponse = await CookieClient.GetAsync(redirectLocation.OriginalString);

            Stream stream = await redirectedResponse.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);

            Assert.NotNull(doc.GetElementbyId("accordion"));

            string dbValue;
            Assert.True(TryGetDbValue("UserName", out dbValue));
            Assert.Equal(NewUserName, dbValue);
        }

        async Task ThenNotSuccessfulUserNameChanged()
        {
            Stream stream = await Response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);

            using (var connection = Startup.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT UserName FROM AspNetUsers";
                    var dbReader = command.ExecuteReader();

                    Assert.True(dbReader.Read());

                    Assert.Equal(User.UserName, dbReader["UserName"]);
                }

                connection.Close();
            }
        }
    }
}
