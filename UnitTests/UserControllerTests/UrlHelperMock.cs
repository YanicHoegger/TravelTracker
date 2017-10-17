using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

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

        public ActionContext ActionContext => throw new NotImplementedException();

        public string Action(UrlActionContext actionContext)
        {
            throw new NotImplementedException();
        }

        public string Content(string contentPath)
        {
            throw new NotImplementedException();
        }

        public bool IsLocalUrl(string url)
        {
            throw new NotImplementedException();
        }

        public string Link(string routeName, object values)
        {
            throw new NotImplementedException();
        }

        public string RouteUrl(UrlRouteContext routeContext)
        {
            var userNameRouteUrl = AssertUrlHelper.Users(routeContext.RouteName, routeContext.Values);

			//Assert.Equal(_userName, userNameFromRouteUrl); //TODO: Reactivate as soon AssertUrlHelper works

			return _returnValue;
        }
    }
}
