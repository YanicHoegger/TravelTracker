using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using IntegrationTests.TestStartups;
using Xunit;

namespace IntegrationTests.UserControllerTests
{
    public class ChangePasswordTests : UserControllerTestsBase
    {
        [Fact]
        public async Task ChangePasswordSuccessfulTest()
        {
            GivenRightPasswordValues();

            await WhenChangePassword();

            await ThenSuccessfulChangedPassword();
        }

        [Fact]
        public async Task ChangePasswordNotSuccessfulTest()
        {
            GivenWrongPasswordValues();

            await WhenChangePassword();

            await ThenSuccessfulChangedPassword();
        }

        string currentPassword;
        string newPassword;
        string retypeNewPassword;

        HttpResponseMessage Response;

        void GivenRightPasswordValues()
        {
            currentPassword = Password;
            newPassword = "BBbb22";
            retypeNewPassword = newPassword;
        }

        void GivenWrongPasswordValues()
        {
            
        }

        async Task WhenChangePassword()
        {
            var formData = new Dictionary<string, string>
              {
                {"__RequestVerificationToken", await GetAntiForgeryToken()},
                {"NewPassword.CurrentPassword", currentPassword},
                {"NewPassword.NewPassword", newPassword},
                {"NewPassword.RetypeNewPassword", retypeNewPassword}
              };

            HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, $"traveller/{User.UserName}/ChangePassword")
            {
                Content = new FormUrlEncodedContent(formData)
            };


            Response = await Client.SendAsync(postRequest);
        }

        async Task ThenSuccessfulChangedPassword()
        {
            Stream stream = await Response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);
        }
    }
}
