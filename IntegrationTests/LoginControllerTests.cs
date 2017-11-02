using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using IntegrationTests.TestStartups;
using IntegrationTests.Utilities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests
{
    public class LoginControllerTests : TestBase<MemoryDbContextStartUp>
    {
        [Fact]
        public async Task LoginWithWrongPasswordNotSuccessfulTest()
        {
            await GivenAccount();

            await WhenLoginWithWrongPasswordAsync();

            await ThenNotSuccessfullyLogedInAsync();
        }

        [Fact]
        public async Task LoginWithWrongUserNameNotSuccessfulTest()
        {
            await GivenAccount();

            await WhenLoginWithWrongUserNameAsync();

            await ThenNotSuccessfullyLogedInAsync();
        }

        [Fact]
        public async Task RightLoginIsSuccessfulTest()
        {
            await GivenAccount();

            await WhenRightLoginAsync();

            await ThenSuccessfullyLogedInAsync();
        }

        [Fact]
        public async Task WhenLogOutThenSuccessfullyLogedOutTest()
        {
            await GivenLogedIn();

            await WhenLogOut();

            ThenSuccessfullyLogedOutAsync();
        }

        IdentityUser user = new IdentityUser()
		{
			UserName = "test",
			Email = "test@test.com"
		};
        string validPassword = "ValidPassword123";

        HttpResponseMessage response;

        async Task GivenAccount()
        {
            await AccountHelper.CreateUserAsync(user, validPassword);
        }

        async Task GivenLogedIn()
        {
            await GivenAccount();

            AccountHelper.LoginUser(user);
        }

        async Task WhenLoginWithWrongPasswordAsync()
        {
            await Login(user.Email, "wrong password");
        }

        async Task WhenLoginWithWrongUserNameAsync()
        {
            await Login("wrong username", validPassword);
        }

        async Task WhenRightLoginAsync()
        {
            await Login(user.Email, validPassword);
        }

        async Task WhenLogOut()
        {
            var content = new StringContent("");
            response = await Client.PostAsync("Login/Logout", content);
        }

        async Task Login(string email, string password)
        {
			var formData = new Dictionary<string, string>
              {
            	{"Email", email},
            	{"Password", password}
              };

			HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, "Login/Login")
			{
				Content = new FormUrlEncodedContent(formData)
			};


			response = await Client.SendAsync(postRequest);
        }

        async Task ThenNotSuccessfullyLogedInAsync()
        {
            //Throws Exception if not success
            response.EnsureSuccessStatusCode();

			Stream stream = await response.Content.ReadAsStreamAsync();
			HtmlDocument doc = new HtmlDocument();
			doc.Load(stream);

            Assert.NotNull(doc.GetElementbyId("LoginForm"));
        }

        async Task ThenSuccessfullyLogedInAsync()
        {
            //Throws Exception if not success
            response.EnsureSuccessStatusCode();

            var cookies = CookiesHelper.ExtractCookiesFromResponse(response);
            Assert.True(cookies.ContainsKey(".AspNetCore.Identity.Application"));
            Assert.False(string.IsNullOrEmpty(cookies[".AspNetCore.Identity.Application"]));

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Redirect", content);
            Assert.Contains(user.UserName, content);
        }

        void ThenSuccessfullyLogedOutAsync()
        {
            AssertResponse.Redirect(response, "/");

            var cookies = CookiesHelper.ExtractCookiesFromResponse(response);
            Assert.True(cookies.ContainsKey(".AspNetCore.Identity.Application"));
            Assert.Equal("", cookies[".AspNetCore.Identity.Application"]);
        }
    }
}
