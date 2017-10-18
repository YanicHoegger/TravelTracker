using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using TravelTracker.Controllers;
using TravelTracker.User;
using Xunit;

namespace UnitTests.UserControllerTests
{
    public class ChangePasswordTests
    {
		[Fact]
		public async Task ChangePasswordNotSuccessfulWhenModelStateErrorTest()
		{
			GivenUserControllerWithModelStateError();

			await ChangePassword();

			ThenResultWithModelStateError(errorMessage);
		}

		[Fact]
		public async Task ChangePasswordNotSuccessfulWhenChangePasswordInUserManagerNotSucceedTest()
		{
			GivenUserControllerWhereChangePasswordInUserManagerNotSucceed();

			await ChangePassword();

			ThenResultWithModelStateError(errorMessage);
		}

		[Fact]
		public async Task ChangePasswordSuccessfulTest()
		{
			GivenUserController();

			await ChangePassword();

			ThenSuccess();
		}

		UserController userController;
		UserDetailsViewModel viewModel;
		IActionResult result;

		const string errorMessage = "Error";

		void GivenUserControllerWithModelStateError()
		{
			userController = new UserController(null);
			userController.ModelState.AddModelError("", errorMessage);
		}

		void GivenUserControllerWhereChangePasswordInUserManagerNotSucceed()
		{
			var error = new IdentityError()
			{
				Description = errorMessage
			};
			var userManagerMock = new Mock<UserManagerMock>();
            userManagerMock.Setup(x => x.ChangePasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
						   .ReturnsAsync(IdentityResult.Failed(error));

			userController = new UserController(userManagerMock.Object);


            viewModel = new UserDetailsViewModel()
            {
                NewPassword = new NewPasswordViewModel()
                {
                    CurrentPassword = "",
                    NewPassword = ""
                }
			};
		}

        void GivenUserController()
        {
			var userManagerMock = new Mock<UserManagerMock>();
			userManagerMock.Setup(x => x.ChangePasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

			userController = new UserController(userManagerMock.Object);


			viewModel = new UserDetailsViewModel()
			{
				NewPassword = new NewPasswordViewModel()
				{
					CurrentPassword = "",
					NewPassword = ""
				}
			};
        }

        async Task ChangePassword()
        {
            result = await userController.ChangePassword(new IdentityUser(), viewModel);
        }

		void ThenResultWithModelStateError(string error)
		{
			var resultAsViewResult = Assert.IsType<ViewResult>(result);

			AssertModelStateDictionary(resultAsViewResult.ViewData.ModelState, error);
			AssertModelStateDictionary(userController.ModelState, error);
		}

		void AssertModelStateDictionary(ModelStateDictionary dictionary, string error)
		{
			Assert.False(dictionary.IsValid);
			Assert.Equal(error, dictionary.GetSingleErrorMessage());
		}

		void ThenSuccess()
		{
			var resultAsViewResult = Assert.IsType<ViewResult>(result);

			Assert.True(resultAsViewResult.ViewData.ModelState.IsValid);
			Assert.True(userController.ModelState.IsValid);
		}
    }
}
