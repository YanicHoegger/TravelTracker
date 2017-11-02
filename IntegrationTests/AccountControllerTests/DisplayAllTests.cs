using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using IntegrationTests.TestStartups;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.AccountControllerTests
{
    public class DisplayAllTests : TestBase<MemoryDbContextStartUp>
    {
        [Fact]
        public async Task DisplayAllAccountsTest()
        {
            await GivenAccountsAndLogedInOne();

            await WhenDisplayAll();

            await ThenDisplayAll();
        }

        HttpResponseMessage Response;
        List<IdentityUser> users = new List<IdentityUser>()
        {
            new IdentityUser()
            {
                UserName = "test1",
                Email = "test1@test.com"
            },
            new IdentityUser()
            {
                UserName = "test2",
                Email = "test2@test.com"
            }
        };

        async Task GivenAccountsAndLogedInOne()
        {
            var userManager = (UserManager<IdentityUser>)Server.Host.Services.GetService(typeof(UserManager<IdentityUser>));
   
            foreach(var user in users)
            {
                await userManager.CreateAsync(user);
            }

            AccountHelper.LoginUserAsAdmin(users[0]);
        }

        async Task WhenDisplayAll()
        {
            Response = await Client.GetAsync("Account/DisplayAll");
        }

        async Task ThenDisplayAll()
        {
            //TODO: Refactoring --> Most tests use Response.EnsureSuccessStatusCode() and load into html document
            //Throws Exception if not success
            Response.EnsureSuccessStatusCode();

            Stream stream = await Response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);

            var listElement = doc.GetElementbyId("accountList");
            var listItemElements = listElement.Elements("li");

            Assert.Equal(2, listItemElements.Count());
        }
    }
}
