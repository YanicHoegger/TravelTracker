using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TravelTracker.Controllers;
using TravelTracker.User;
using Xunit;

namespace UnitTests.AccountControllerTests
{
    public class DisplayAllAccountsTests
    {
        [Fact]
        public void ShowAllAccountsFromUserManagerTest()
        {
            GivenAccountControllerWithUsers();

            WhenDisplayAll();

            ThenShowAllAccounts();
        }

        [Fact]
        public void ShowNoAccountsWhenUserManagerReturnsEmptyList()
        {
            GivenAccountControllerWhereUserManagerReturnsNoUsers();

            WhenDisplayAll();

            ThenShowNoAccounts();
        }

        AccountController _accountController;
        IActionResult _result;

        IQueryable<IdentityUser> _users = new List<IdentityUser>
                {
                    new IdentityUser
                    {
                        UserName = "Test1",
                        Email = "test1@test.it"
                    },
                    new IdentityUser
                    {
                        UserName = "Test2",
                        Email = "test2@test.it"
                    }
                }.AsQueryable();

        void GivenAccountControllerWithUsers()
        {
            var userManagerMock = new Mock<UserManagerMock>();
            userManagerMock.Setup(x => x.Users).Returns(_users);

            _accountController = new AccountController(userManagerMock.Object)
            {
                Url = new UrlHelperMock()
            };
        }

        void GivenAccountControllerWhereUserManagerReturnsNoUsers()
        {
            var userManagerMock = new Mock<UserManagerMock>();
		    userManagerMock.Setup(x => x.Users).Returns(new List<IdentityUser>().AsQueryable());

		    _accountController = new AccountController(userManagerMock.Object);
        }

        void WhenDisplayAll()
        {
            _result = _accountController.DisplayAll();
        }

        void ThenShowAllAccounts()
        {
            var viewModelList = GetAndAssertViewModelList();

            AssertIdentityUserAndUserViewModel(_users.First(), viewModelList.First());
            AssertIdentityUserAndUserViewModel(_users.ElementAt(1), viewModelList.ElementAt(1));
        }

        void AssertIdentityUserAndUserViewModel(IdentityUser identityUser, UserViewModel viewModel)
        {
            Assert.Equal(identityUser.UserName, viewModel.UserName);
			//TODO: Test for viewModel.RouteUrl
		}

        void ThenShowNoAccounts()
        {
			var viewModelList = GetAndAssertViewModelList();

            Assert.Empty(viewModelList);
        }

        IEnumerable<UserViewModel> GetAndAssertViewModelList()
        {
			var resultAsViewResult = Assert.IsType<ViewResult>(_result);
			var viewModelList = resultAsViewResult.Model as IEnumerable<UserViewModel>;
			Assert.NotNull(viewModelList);

            return viewModelList;
        }
    }
}
