
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
    public class ChangeUserNameTests
    {
        [Fact]
        public async Task ChangeUserNameNotSuccessfulWhenModelStateErrorTest()
        {
            GivenUserControllerWithModelStateError();

            await ChangeUserName();

            ThenResultWithModelStateError(errorMessage);
        }

        [Fact]
        public async Task ChangeUserNameNotSuccessfulWhenUpdateUserNotSucceedTest()
        {
            GivenUserControllerWhereUpdateAsyncNotSucceed();

			await ChangeUserName();

			ThenResultWithModelStateError(errorMessage);
        }

        [Fact]
        public async Task ChangeUserNameSuccessfulTest()
        {
            GivenUserController();

            await ChangeUserName();

            ThenSuccess();
        }

        UserController userController;
        UserDetailsViewModel viewModel;
        IActionResult result;

        const string errorMessage = "Error";
        const string routeUrl = "routeUrl";

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
                NewUserName = new NewUserNameViewModel()
                {
                    NewUserName = ""
                }
            };
        }

        void GivenUserController()
        {
			var userManagerMock = new Mock<UserManagerMock>();
			userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>()))
                           .ReturnsAsync(IdentityResult.Success);
            
            var userName = "userName";

			userController = new UserController(userManagerMock.Object);
            userController.Url = new UrlHelperMock(routeUrl, userName);

			viewModel = new UserDetailsViewModel()
			{
				NewUserName = new NewUserNameViewModel()
				{
					NewUserName = userName
				}
			};
        }

        async Task ChangeUserName()
        {
            result = await userController.ChangeUserName(new IdentityUser(), viewModel);
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
            var resultAsRedirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(routeUrl, resultAsRedirectResult.Url);
        }
    }
}
