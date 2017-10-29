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
            GivenRightNewUserName();

            await WhenChangeUserName();

            await ThenSuccessfulUserNameChanged();
        }

        [Fact]
        public async Task ChangeUserNameNotSuccessfulTest()
        {
            await GivenAlreayUsedNewUserName();

            //TODO: Check how no exception is thrown when update db
            await WhenChangeUserName();

            ThenNotSuccessfulUserNameChanged();
        }

        string NewUserName;
        HttpResponseMessage Response;

        void GivenRightNewUserName()
        {
            NewUserName = "newName";
        }

        async Task GivenAlreayUsedNewUserName()
        {
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
            redirectedResponse.EnsureSuccessStatusCode();

            string dbValue;
            Assert.True(TryGetDbValue("UserName", out dbValue));
            Assert.Equal(NewUserName, dbValue);
        }

        void ThenNotSuccessfulUserNameChanged()
        {
            string dbValue;
            Assert.True(TryGetDbValue("UserName", out dbValue));
            Assert.Equal(User.UserName, dbValue);
        }
    }
}
