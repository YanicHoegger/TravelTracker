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
    public class DeleteAccountTests
    {
        [Fact]
        public async Task DeleteUserAccountFromUserManagerSuccessfulTest()
		{
			GivenAccountControllerWithUser();

            await WhenDeleteUserAsync();

            ThenUserDoesNotExistInUserManager();
		}

        [Fact]
        public async Task TryDeleteUserThatDoesNotExistTest()
        {
            GivenAccountController();

            await WhenDeletUserThatDoesNotExistThenThrowArgumentException();
        }

        [Fact]
        public async Task TryDeleteUserWhenUserManagerDoesNotDeleteUser()
        {
            GivenAccountControllerWitchCanNotDelete();

            await WhenDeleteUserAsyncThenUnauthorizedAccessExceptionIsThrown();
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
            await _accountController.DeleteUser(user1.UserName);
        }

        async Task WhenDeleteUserAsyncThenUnauthorizedAccessExceptionIsThrown()
        {
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _accountController.DeleteUser(user1.UserName));
            Assert.Equal($"Can not delete user. Reson(s): {identityError1.Description}, {identityError2.Description}", exception.Message);
        }

        async Task WhenDeletUserThatDoesNotExistThenThrowArgumentException()
        {
            const string userName = "NotExistendName";

            var argumentException = await Assert.ThrowsAsync<ArgumentException>(() => _accountController.DeleteUser(userName));
            Assert.Equal($"'{userName}' is not an existend UserName", argumentException.Message);
        }

        void ThenUserDoesNotExistInUserManager()
        {
            Assert.DoesNotContain(user1, _userManager.Users);
            Assert.Equal(1, _userManager.Users.Count());
        }
    }
}
