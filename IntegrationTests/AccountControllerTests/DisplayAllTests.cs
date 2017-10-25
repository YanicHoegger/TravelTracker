using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using IntegrationTests.TestStartups;
using IntegrationTests.Utilities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.AccountControllerTests
{
    public class DisplayAllTests : TestBase<MemoryDbContextStartUp>
    {
        [Fact]
        public async Task DisplayAllAccountsTest()
        {
            await GivenAccounts();

            await WhenDisplayAll();

            await ThenDisplayAll();
        }

        [Fact]
        public async Task DisplayNoAccountsTest()
        {
            GivenNoAccounts();

            await WhenDisplayAll();

            await ThenDisplayNone();
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

        async Task GivenAccounts()
        {
            var accountHelper = new AccountHelper(Server);
            //TODO: Check how to rund multiple in foreach
            var awaiter = new List<Task>();

            foreach(var user in users)
            {
                awaiter.Add(accountHelper.CreateUserAsync(user, "ASas12"));
            }

            foreach(var task in awaiter)
            {
                await task;
            }
        }

        void GivenNoAccounts()
        {
            //Nothing to do here
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

            var listItemElements = await GetListItems();
        }

        async Task ThenDisplayNone()
        {
            //Throws Exception if not success
            Response.EnsureSuccessStatusCode();

            Assert.Empty(await GetListItems());
        }

        async Task<IEnumerable<HtmlNode>> GetListItems()
        {
            Stream stream = await Response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);

            //TODO: Set an id for list
            var listElement = doc.GetElementbyId("id");
            //var listItemElements = listElement.ChildNodes.Where(x => x.Attributes.Contains());

            return null;
        }
    }
}
