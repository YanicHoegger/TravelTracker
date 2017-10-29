using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.UserControllerTests
{
    public class ChangePasswordTests : UserControllerTestsBase
    {
        public ChangePasswordTests()
        {
            TryGetDbValue("PasswordHash", out oldPasswordHash);
        }

        [Fact]
        public async Task ChangePasswordSuccessfulTest()
        {
            GivenRightPasswordValues();

            await WhenChangePassword();

            ThenSuccessfulChangedPassword();
        }

        [Fact]
        public async Task ChangePasswordNotSuccessfulTest()
        {
            GivenWrongPasswordValues();

            await WhenChangePassword();

            ThenPasswordNotChanged();
        }

        readonly string oldPasswordHash;

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
            currentPassword = Password;
            newPassword = "nouppercase";
            retypeNewPassword = "somethingDifferent";
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

            Response = await CookieClient.PostAsync($"traveller/{User.UserName}/ChangePassword", formData);
        }

        void ThenSuccessfulChangedPassword()
        {
            Response.EnsureSuccessStatusCode();

            string dbValue;
            Assert.True(TryGetDbValue("PasswordHash", out dbValue));
            Assert.NotEqual(oldPasswordHash, dbValue);
        }

        void ThenPasswordNotChanged()
        {
            //Response.EnsureSuccessStatusCode();

            var temp = Response.Content.ReadAsStringAsync().Result;

            string dbValue;
            Assert.True(TryGetDbValue("PasswordHash", out dbValue));
            Assert.Equal(oldPasswordHash, dbValue);
        }
    }
}
