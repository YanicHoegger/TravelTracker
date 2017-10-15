using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace UnitTests.AccountControllerTests
{
    public class UrlHelperMock : IUrlHelper
    {
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
            if (!routeContext.RouteName.Equals("users"))
			{
				throw new ArgumentException("The used router should be 'users'", nameof(routeContext.RouteName));
			}

			//TODO: Check for routeContext.Values

			return "someString";
        }
    }
}
