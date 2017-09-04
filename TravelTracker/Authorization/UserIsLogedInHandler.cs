using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TravelTracker.Authorization
{
    public class UserIsLogedInHandler : AuthorizationHandler<UserIsLogedInRequirement>
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;

        public UserIsLogedInHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIsLogedInRequirement requirement)
        {
            var authorizationFilterContext = (AuthorizationFilterContext)context.Resource;
            if(authorizationFilterContext == null)
            {
                return;   
            }
            var userNameFromUrl = (string)authorizationFilterContext.RouteData.Values["username"];

            var user = await _userManager.FindByNameAsync(userNameFromUrl);

            if(user != null)
            {
                _signInManager.IsSignedIn(context.User);
            }
        }
    }
}
