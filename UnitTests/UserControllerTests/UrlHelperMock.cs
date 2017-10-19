using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.UserControllerTests
{
    public class UrlHelperMock : IUrlHelper
    {
		readonly string _returnValue;
        readonly string _userName;

        public UrlHelperMock(string returnValue, string userName)
        {
            _returnValue = returnValue;
            _userName = userName;
        }

        public ActionContext ActionContext => throw new NotSupportedException();

        public string Action(UrlActionContext actionContext)
        {
            throw new NotSupportedException();
        }

        public string Content(string contentPath)
        {
            throw new NotSupportedException();
        }

        public bool IsLocalUrl(string url)
        {
            throw new NotSupportedException();
        }

        public string Link(string routeName, object values)
        {
            throw new NotSupportedException();
        }

        public string RouteUrl(UrlRouteContext routeContext)
        {
            var userNameFromRouteUrl = AssertUrlHelper.Users(routeContext.RouteName, routeContext.Values);

			Assert.Equal(_userName, userNameFromRouteUrl);

			return _returnValue;
        }
    }
}
