using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.UserControllerTests
{
    public class ChangeEmailTests : UserControllerTestsBase
    {
        [Fact]
        public async Task ChangeEmailSuccessfulTest()
        {
            GivenRightNewEmailValue();

            await WhenChangeEmail();

            ThenEmailChanged();
        }

        [Fact]
        public async Task ChangeEmailNotSuccessfulTest()
        {
            GivenWrongNewEmailValue();

            await WhenChangeEmail();

            ThenEmailNotChanged();
        }

        string newEmail;

        HttpResponseMessage Response;

        void GivenRightNewEmailValue()
        {
            newEmail = "something@new.com";
        }

        void GivenWrongNewEmailValue()
        {
            newEmail = "somethingWithoutAt";
        }

        async Task WhenChangeEmail()
        {
            var formData = new Dictionary<string, string>
              {
                {"__RequestVerificationToken", await GetAntiForgeryToken()},
                {"NewEmail.NewEmail", newEmail}
              };

            Response = await CookieClient.PostAsync($"traveller/{User.UserName}/ChangeEmail", formData);
        }

        void ThenEmailChanged()
        {
            Response.EnsureSuccessStatusCode();

            string dbValue;
            Assert.True(TryGetDbValue("Email", out dbValue));
            Assert.NotEqual(newEmail, dbValue);
        }

        void ThenEmailNotChanged()
        {
            Response.EnsureSuccessStatusCode();

            string dbValue;
            Assert.True(TryGetDbValue("Email", out dbValue));
            Assert.NotEqual(User.Email, dbValue);
        }
    }
}
