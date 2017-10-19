using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.AccountControllerTests
{
    public class UrlHelperMock : IUrlHelper
    {
        readonly string _returnValue;
        readonly IEnumerable<string> _userNames;

        public UrlHelperMock(string returnValue, IEnumerable<string> userNames)
        {
            _userNames = userNames;
            _returnValue = returnValue;
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
            var userNameRouteUrl = AssertUrlHelper.Users(routeContext.RouteName, routeContext.Values);

			Assert.Contains(userNameRouteUrl, _userNames);

			return _returnValue;
        }
    }
}
