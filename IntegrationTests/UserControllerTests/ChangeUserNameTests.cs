using System.Threading.Tasks;
using IntegrationTests.TestStartups;
using Xunit;

namespace IntegrationTests.UserControllerTests
{
    public class ChangeUserNameTests : TestBase<SqlliteDbContextStartup>
    {
        [Fact]
        public async Task ChangeUserNameSuccessfulTest()
        {
            
        }

        [Fact]
        public async Task ChangeUserNameNotSuccessfulTest()
        {
            
        }
    }
}
