using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.Controllers;
using TravelTracker.User;
using Xunit;

namespace UnitTests.UserControllerTests
{
    public class CheckUserExistsTests
    {
        [Fact]
        public void IndexCheckUserExistTest()
        {
            GvienUserController();

            WhenIndex();

            ThenNotFoundResult();
        }

        [Fact]
        public async Task ChangeUserNameCheckUserExistTest()
		{
			GvienUserController();

            await WhenAction(userController.ChangeUserName);

			ThenNotFoundResult();
		}

		[Fact]
		public async Task ChangeEmailCheckUserExistTest()
		{
			GvienUserController();

			await WhenAction(userController.ChangeEmail);

			ThenNotFoundResult();
		}

		[Fact]
		public async Task ChangePasswordCheckUserExistTest()
		{
			GvienUserController();

			await WhenAction(userController.ChangePassword);

			ThenNotFoundResult();
		}

		//Class could be simplyfied with this method, but in terms of better readable it is not used
        //TODO: Check lambda and object relation
        /*async Task UserControllerActionCheckUserExistTest(Func<UserController, Func<IdentityUser, UserDetailsViewModel, Task<IActionResult>>> userControllerActionGetter)
        {
			GvienUserController();

            await WhenAction(userControllerActionGetter(userController));

			ThenNotFoundResult();
        }*/

        UserController userController;
        IActionResult result;

        void GvienUserController()
        {
            userController = new UserController(null);
        }

        void WhenIndex()
        {
            result = userController.Index(null);
        }

        async Task WhenAction(Func<IdentityUser, UserDetailsViewModel, Task<IActionResult>> userControllerAction)
		{
            result = await userControllerAction(null, null);
		}

        void ThenNotFoundResult()
        {
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
