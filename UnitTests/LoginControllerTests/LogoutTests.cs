using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TravelTracker.Controllers;
using Xunit;

namespace UnitTests.LoginControllerTests
{
    public class LogoutTests
    {
        [Fact]
        public async Task LogoutSuccessful()
        {
            GivenLoginController();

            await WhenLogout();

            ThenLogedOut();
        }

        LoginController loginController;
        IActionResult result;

        void GivenLoginController()
        {
            var signInManagerMock = new Mock<SignInManagerMock>();
            signInManagerMock.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);

            loginController = new LoginController(null, signInManagerMock.Object);
        }

        async Task WhenLogout()
        {
            result = await loginController.Logout();
        }

        void ThenLogedOut()
        {
			//Nothing can go wrong here, if not logged out, user will see that
			//Unlikly that logout goes wrong

            var resultAsRedirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("~/", resultAsRedirectResult.Url);
        }
    }
}
