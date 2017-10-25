using System;
using System.Threading.Tasks;
using IntegrationTests.TestStartups;
using Xunit;

namespace IntegrationTests.AccountControllerTests
{
    public class DeleteUserAccountTests : TestBase<SqlliteDbContextStartup>
    {
        [Fact]
        public async Task DeleteUserAccountTest()
        {
            await GivenAccount();

            await WhenDeleteAccount();

            await ThenAccountDeleted();
        }

        async Task GivenAccount()
        {
            throw new NotImplementedException();
        }

        async Task WhenDeleteAccount()
        {
            
        }

        async Task ThenAccountDeleted()
        {
            //TODO: Check DB
        }
    }
}
