using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Language.Flow;
using TravelTracker.Controllers;
using TravelTracker.User;
using Xunit;

namespace UnitTests.LoginControllerTests
{
    public class LoginTests
    {
        [Fact]
        public async Task LoginSuccessfulTest()
        {
            GivenLoginController();

            await WhenLogin();

            ThenSuccess();
        }

        [Fact]
        public async Task LoginNotSuccessfulWhenModelStateErrorTest()
        {
            GivenLoginControllerWithModelStateError();

            await WhenLogin();

            ThenResultWithModelStateError();
        }

        [Fact]
        public async Task LoginNotSuccessfulWhenCanNotFindEmailTest()
        {
            GivenLoginControllerWhereUserManagerCanNotFindUser();

            await WhenLogin();

            ThenResultWithModelStateError();
        }

        [Fact]
        public async Task LoginNotSuccessfulWhenPasswordWrongTest()
        {
			GivenLoginControllerWhereSigninManagerCanNotSignin();

			await WhenLogin();

			ThenResultWithModelStateError();
        }

        LoginController loginController;
        IActionResult result;
        LoginViewModel viewModel;

        const string linkUrl = "linkUrl";

        void GivenLoginController()
        {
            var user = new IdentityUser()
            {
                UserName = "user"
            };

            var userManagerMock = new Mock<UserManagerMock>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            var signinManagerMock = new Mock<SignInManagerMock>();
            signinManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            //TODO: Check how to handle same definition of two different libraries --> Microsoft.AspNetCore.Identity.SignInResult.Success to SignInResult.Success

            loginController = new LoginController(userManagerMock.Object, signinManagerMock.Object)
            {
                Url = new UrlHelperMock(user.UserName, linkUrl)
            };

            viewModel = new LoginViewModel();
		}

        void GivenLoginControllerWithModelStateError()
        {
            loginController = new LoginController(null, null);
            loginController.ModelState.AddModelError("", "Error");
        }

        void GivenLoginControllerWhereUserManagerCanNotFindUser()
        {
			var userManagerMock = new Mock<UserManagerMock>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null);

			loginController = new LoginController(userManagerMock.Object, null);

            viewModel = new LoginViewModel();
        }

        void GivenLoginControllerWhereSigninManagerCanNotSignin()
        {
			var userManagerMock = new Mock<UserManagerMock>();
			userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());

			var signinManagerMock = new Mock<SignInManagerMock>();
			signinManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
			//TODO: Check how to handle same definition of two different libraries --> Microsoft.AspNetCore.Identity.SignInResult.Success to SignInResult.Success

			loginController = new LoginController(userManagerMock.Object, signinManagerMock.Object);

			viewModel = new LoginViewModel();
        }

        async Task WhenLogin()
        {
            result = await loginController.Login(viewModel);
        }

        void ThenResultWithModelStateError()
        {
            var resultAsViewComponentResult = Assert.IsType<ViewComponentResult>(result);
            Assert.False(resultAsViewComponentResult.ViewData.ModelState.IsValid);

			Assert.False(loginController.ModelState.IsValid);
        }

        void ThenSuccess()
        {
            var resultAsContentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal($"Redirect {linkUrl}", resultAsContentResult.Content);
        }
    }
}
