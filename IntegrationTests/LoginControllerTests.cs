using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IntegrationTests.Utilities;
using Xunit;

namespace IntegrationTests
{
    public class LoginControllerTests : IClassFixture<MemoryTestServerClientFixture>
    {
        readonly MemoryTestServerClientFixture _testServerClient;

        public LoginControllerTests(MemoryTestServerClientFixture testServerClient)
        {
            _testServerClient = testServerClient;
        }

        [Fact]
        public async Task RightLoginIsSuccessfulTest()
        {
            await WhenLogin();

            await ThenSuccessfullyLogedInAsync();
        }

        [Fact]
        public async Task WhenLogOutThenSuccessfullyLogedOutTest()
        {
            GivenLogedIn();

            await WhenLogOut();

            ThenSuccessfullyLogedOutAsync();
        }

        HttpResponseMessage response;

        void GivenLogedIn()
        {
            _testServerClient.AccountHelper.LoginUser(_testServerClient.User);
        }

        async Task WhenLogin()
        {
            var formData = new Dictionary<string, string>
              {
                {"Email", _testServerClient.User.Email},
                {"Password", _testServerClient.UserPassword}
              };

            HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, "Login/Login")
            {
                Content = new FormUrlEncodedContent(formData)
            };


            response = await _testServerClient.Client.SendAsync(postRequest);
        }

        async Task WhenLogOut()
        {
            var content = new StringContent("");
            response = await _testServerClient.Client.PostAsync("Login/Logout", content);
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
            Assert.Contains(_testServerClient.User.UserName, content);
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
