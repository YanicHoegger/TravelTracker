using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using TravelTracker.Controllers;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.AccountControllerTests
{
    //TODO: Assert Exception
    public class DeleteAccountTests
    {
		[Fact]
		public async Task DeleteUserAccountFromUserManagerTest()
		{
			GivenAccountControllerWithUser();

            await WhenDeleteUserAsync();

            ThenUserDoesNotExistInUserManager();
		}

        [Fact]
        public async Task TryDeleteUserThatDoesNotExistTest()
        {
            GivenAccountController();

            await WhenDeletUserThatDoesNotExist();

            ThenArgumentExceptionIsThrown();
        }

        [Fact]
        public async Task TryDeleteUserWhenUserManagerDoesNotDeleteUser()
        {
            GivenAccountControllerWitchCanNotDelete();

            await WhenDeleteUserAsync();

            ThenUnauthorizedAccessExceptionThrown();
        }

        DeleteAccountTestUserManagerMock _userManager;
        AccountController _accountController;

        IdentityUser user1 = new IdentityUser
        {
            UserName = "Test1",
            Email = "test1@test.it"
        };

        IdentityUser user2 = new IdentityUser
        {
            UserName = "Test2",
            Email = "test2@test.it"
        };

		IdentityError identityError1 = new IdentityError()
		{
			Description = "Wrong something"
		};
		IdentityError identityError2 = new IdentityError()
		{
			Description = "Wrong an other thing"
		};

        void GivenAccountControllerWithUser()
        {
            var userList = new List<IdentityUser>();
            userList.Add(user1);
            userList.Add(user2);

            _userManager = new DeleteAccountTestUserManagerMock()
            {
                Users = userList
            };

            _accountController = new AccountController(_userManager);
        }

        Exception _exception;

        void GivenAccountController()
        {
			var userManagerMock = new Mock<UserManagerMock>();
            userManagerMock.Setup(x => x.Users).Returns<IQueryable<IdentityUser>>(null);

            _accountController = new AccountController(userManagerMock.Object);
        }

        void GivenAccountControllerWitchCanNotDelete()
        {
            var userManagerMock = new Mock<UserManagerMock>();

            userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());


            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>()))
                           .ReturnsAsync(IdentityResult.Failed(identityError1, identityError2));

            _accountController = new AccountController(userManagerMock.Object);
        }

        async Task WhenDeleteUserAsync()
        {
			try
			{
				await _accountController.DeleteUser(user1.UserName);
			}
			catch (Exception e)
			{
				_exception = e;
			}
        }

        async Task WhenDeletUserThatDoesNotExist()
        {
            try
            {
                await _accountController.DeleteUser("NotExistendName");
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        void ThenUserDoesNotExistInUserManager()
        {
            Assert.DoesNotContain(user1, _userManager.Users);
            Assert.Equal(1, _userManager.Users.Count());
        }

        void ThenArgumentExceptionIsThrown()
        {
            var argumentException = Assert.IsType<ArgumentException>(_exception);
            Assert.Equal("'NotExistendName' is not an existend UserName", _exception.Message);
        }

        void ThenUnauthorizedAccessExceptionThrown()
        {
            var unauthorizedAccessException = Assert.IsType<UnauthorizedAccessException>(_exception);
            Assert.Equal($"Can not delete user. Reson(s): {identityError1.Description}, {identityError2.Description}", _exception.Message);
        }
    }
}
