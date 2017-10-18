﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Xunit;

namespace UnitTests.LoginControllerTests
{
    public class UrlHelperMock : IUrlHelper
    {
		readonly string _userName, _returnValue;

		public UrlHelperMock(string userName, string returnValue)
		{
			_userName = userName;
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
            var userNameFromRouteUrl = AssertUrlHelper.Users(routeName, values);

            Assert.Equal(_userName, userNameFromRouteUrl); //TODO: Reactivate as soon AssertUrlHelper works

            return _returnValue;
        }

        public string RouteUrl(UrlRouteContext routeContext)
        {
            throw new NotSupportedException();
        }
    }
}
