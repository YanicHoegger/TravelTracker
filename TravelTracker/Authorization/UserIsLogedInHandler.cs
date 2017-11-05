using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TravelTracker.Authorization
{
    public class UserIsLogedInHandler : AuthorizationHandler<UserIsLogedInRequirement>
    {
        readonly SignInManager<IdentityUser> _signInManager;

        public UserIsLogedInHandler(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIsLogedInRequirement requirement)
        {
            var authorizationFilterContext = (AuthorizationFilterContext)context.Resource;
            if(authorizationFilterContext == null)
            {
                return Task.CompletedTask;   
            }

            if(_signInManager.IsSignedIn(context.User))
            {
                var roleClaim = context.User.FindFirst(ClaimTypes.Role);
                if(roleClaim != null && roleClaim.Value.Equals("Administrator"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    var userNameOfSignedInUser = context.User.FindFirst(ClaimTypes.Name).Value;
                    var userNameFromUrl = (string)authorizationFilterContext.RouteData.Values["username"];

                    if (userNameOfSignedInUser.Equals(userNameFromUrl, StringComparison.CurrentCultureIgnoreCase))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
		}
    }
}
