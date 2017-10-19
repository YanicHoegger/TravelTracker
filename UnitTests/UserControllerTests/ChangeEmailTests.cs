using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using TravelTracker.Controllers;
using TravelTracker.User;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.UserControllerTests
{
    public class ChangeEmailTests
    {
        [Fact]
        public async Task ChangeEmailNotSuccessfulWhenModelStateErrorTest()
        {
            GivenUserControllerWithModelStateError();

            await ChangeEmail();

            ThenResultWithModelStateError(errorMessage);
        }

        [Fact]
        public async Task ChangeEmailNotSuccessfulWhenUpdateUserNotSucceedTest()
        {
			GivenUserControllerWhereUpdateAsyncNotSucceed();

			await ChangeEmail();

			ThenResultWithModelStateError(errorMessage);
        }

        [Fact]
        public async Task ChangeEmailSuccessfulTest()
        {
            GivenUserController();

            await ChangeEmail();

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

		void GivenUserControllerWhereUpdateAsyncNotSucceed()
		{
			var error = new IdentityError()
			{
				Description = errorMessage
			};
			var userManagerMock = new Mock<UserManagerMock>();
			userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>()))
						   .ReturnsAsync(IdentityResult.Failed(error));

			userController = new UserController(userManagerMock.Object);


			viewModel = new UserDetailsViewModel()
			{
                NewEmail = new NewEmailViewModel()
				{
                    NewEmail = ""
				}
			};
		}

		void GivenUserController()
		{
			var userManagerMock = new Mock<UserManagerMock>();
			userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>()))
						   .ReturnsAsync(IdentityResult.Success);

			userController = new UserController(userManagerMock.Object);

			viewModel = new UserDetailsViewModel()
			{
				NewEmail = new NewEmailViewModel()
				{
					NewEmail = ""
				}
			};
		}

        async Task ChangeEmail()
        {
            result = await userController.ChangeEmail(new IdentityUser(), viewModel);
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
