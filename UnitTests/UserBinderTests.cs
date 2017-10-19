using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing;
using Moq;
using TravelTracker.User;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests
{
    public class UserBinderTests
    {
        [Fact]
        public async Task BindingContextNullTest()
        {
            GivenNoUserMangerUserBinder();

            await WhenBindToNullThenExceptionAsync();
        }

        [Fact]
        public async Task WrongRoutingDataTest()
        {
            GivenNoUserMangerUserBinder();

            await WhenBindToWrongRoutingDataThenException();
        }

        [Fact]
        public async Task UserNotFoundTest()
        {
            GivenUserBinderWhereUserManagerCanNotFindUser();

            await WhenBind();

            ThenResultNull();
        }

        [Fact]
        public async Task UserBindSuccessfulTest()
        {
            GivenUserBinder();

            await WhenBind();

            ThenSuccess();
        }

        UserBinder userBinder;
        ModelBindingContext modelBindingContext;

        IdentityUser user;

        void GivenNoUserMangerUserBinder()
        {
            userBinder = new UserBinder(null);
        }

        void GivenUserBinderWhereUserManagerCanNotFindUser()
        {
            var userManagerMock = new Mock<UserManagerMock>();
            userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null);

            userBinder = new UserBinder(userManagerMock.Object);

			modelBindingContext = CreateModelBindingContext();
        }

        void GivenUserBinder()
        {
            user = new IdentityUser();

			var userManagerMock = new Mock<UserManagerMock>();
			userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

			userBinder = new UserBinder(userManagerMock.Object);

			modelBindingContext = CreateModelBindingContext();
        }

        async Task WhenBind()
        {
            await userBinder.BindModelAsync(modelBindingContext);
        }

        async Task WhenBindToNullThenExceptionAsync()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => userBinder.BindModelAsync(null));
        }

        async Task WhenBindToWrongRoutingDataThenException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => userBinder.BindModelAsync(CreateModelBindingContext(new RouteData())));
        }

        void ThenResultNull()
        {
            Assert.False(modelBindingContext.Result.IsModelSet);
            Assert.Null(modelBindingContext.Result.Model);
        }

        void ThenSuccess()
        {
            Assert.True(modelBindingContext.Result.IsModelSet);
            Assert.Equal(user, modelBindingContext.Result.Model);
        }

		ModelBindingContext CreateModelBindingContext()
        {
            return CreateModelBindingContext(CreateRightRouteData());
        }

        RouteData CreateRightRouteData()
        {
			var routeData = new RouteData();

			var values = new RouteValueDictionary(new { username = "username" });

            routeData.PushState(new Mock<IRouter>().Object, values, new RouteValueDictionary());

            return routeData;
        }

        ModelBindingContext CreateModelBindingContext(RouteData routeData)
        {
            var actionContext = new ActionContext()
            {
                RouteData = routeData
            };

            var modelBindingContextMock = new Mock<ModelBindingContext>();
            modelBindingContextMock.SetupProperty(x => x.Result);
            modelBindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);

            return modelBindingContextMock.Object;
        }
    }
}
