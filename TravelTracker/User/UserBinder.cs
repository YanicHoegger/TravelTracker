using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TravelTracker.User
{
    public class UserBinder : IModelBinder
    {
        readonly UserManager<IdentityUser> _userManager;

        public UserBinder(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var userName = (string)bindingContext.ActionContext.RouteData.Values["username"];

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return;
            }

            bindingContext.Result = ModelBindingResult.Success(user);
        }
    }
}
