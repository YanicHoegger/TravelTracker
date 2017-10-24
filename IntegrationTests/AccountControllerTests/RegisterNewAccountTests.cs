using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Xunit;

namespace IntegrationTests.AccountControllerTests
{
    public class RegisterNewAccountTests : TestBase<SqlliteDbContextStartup>
    {
        [Fact]
        public async Task RegisterNewUserSuccessfulTest()
        {
            GivenValues();

            await WhenRegister();

            await ThenSuccessfulRegistered();
        }

        [Fact]
        public async Task RegisterNewUserWithWrongValuesNotSuccessfulTest()
        {
            GivenWrongValues();

            await WhenRegister();

            await ThenRegisterSiteWithErrors();
        }

        [Fact]
        public async Task RegisterNewUserWithEmptyValuesNotSuccessfulTest()
        {
            GivenEmptyValues();

            await WhenRegister();

            await ThenRegisterSiteWithErrors();
        }

        Dictionary<string, string> formData;
        HttpResponseMessage response;

        void GivenValues()
        {
            CreateFormData("test@test.email", "testUserName", "ASde34", "ASde34");
        }

        void GivenWrongValues()
        {
            //TODO: Use already given username
            CreateFormData("invalidEmail", "test", "as", "sadfaf");
        }

        void CreateFormData(string email, string userName, string password, string retypePassword)
        {
            formData = new Dictionary<string, string>()
            {
                {"Email", email},
                {"UserName", userName},
                {"Password", password},
                {"RetypePassword", retypePassword}
            };
        }

        void GivenEmptyValues()
        {
            formData = new Dictionary<string, string>();
        }

        async Task WhenRegister()
        {
            HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, "Account/Register")
            {
                Content = new FormUrlEncodedContent(formData)
            };


            response = await Client.SendAsync(postRequest);
        }

        async Task ThenSuccessfulRegistered()
        {
            //Throws Exception if not success
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Registering user successful", content);
        }

        async Task ThenRegisterSiteWithErrors()
        {
            //Throws Exception if not success
            response.EnsureSuccessStatusCode();

            Stream stream = await response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);

            Assert.NotNull(doc.GetElementbyId("Email-error"));
            Assert.NotNull(doc.GetElementbyId("UserName-error"));
            Assert.NotNull(doc.GetElementbyId("Password-error"));
            Assert.NotNull(doc.GetElementbyId("RetypePassword-error"));
        }
    }
}
