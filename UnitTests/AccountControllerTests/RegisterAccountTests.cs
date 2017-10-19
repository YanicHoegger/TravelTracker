using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TravelTracker.Controllers;
using TravelTracker.User;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.AccountControllerTests
{
    public class RegisterAccountTests
    {
        [Fact]
        public async Task RegisterSuccessfulTest()
        {
            GivenAccountControllerWhereUserManagerReturnsSuccess();

            await WhenRegisterAsync();

            ThenSuccess();
        }

        [Fact]
		public async Task RegisterNotSuccessfulWhenModelStateErrorTest()
		{
			GivenAccountControllerWithModelStateError();

			await WhenRegisterAsync();

			ThenResultWithModelStateError();
		}

        [Fact]
        public async Task RegisterNotSuccessfuWhenUserManagerReturnsErrorTest()
        {
            GivenAccountControllerWhereUserManagerReturnsErrors();

            await WhenRegisterAsync();

            ThenResultWithModelStateError();
        }

		AccountController _accountController;
		IActionResult _result;

        void GivenAccountControllerWhereUserManagerReturnsSuccess()
        {
			var userManagerMock = new Mock<UserManagerMock>();
			userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

			_accountController = new AccountController(userManagerMock.Object);
        }

        void GivenAccountControllerWithModelStateError()
		{
			_accountController = new AccountController(new Mock<UserManagerMock>().Object);
			_accountController.ModelState.AddModelError("", "Error");
		}

        void GivenAccountControllerWhereUserManagerReturnsErrors()
        {
            var identityError = new IdentityError()
            {
                Code = "Code",
                Description = "Error"
            };

            var userManagerMock = new Mock<UserManagerMock>();
			userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Failed(identityError));

            _accountController = new AccountController(userManagerMock.Object);
        }

		async Task WhenRegisterAsync()
		{
			_result = await _accountController.Register(new RegisterUserViewModel());
		}

		void ThenResultWithModelStateError()
		{
			var resultAsViewResult = Assert.IsType<ViewResult>(_result);
			Assert.False(resultAsViewResult.ViewData.ModelState.IsValid);

			Assert.False(_accountController.ModelState.IsValid);
		}

        void ThenSuccess()
        {
            var resultAsContentResult = Assert.IsType<ContentResult>(_result);
            Assert.Equal("Registering user successful", resultAsContentResult.Content);
        }
    }
}
