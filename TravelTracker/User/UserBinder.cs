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

            var userNameAsString = bindingContext.ActionContext.RouteData.Values["username"] as string;

            if(userNameAsString == null)
            {
                //TODO: Use own Exception --> Declare in issues on github
                throw new ArgumentException("ModelBinder is used with the wrong route");
            }

            var user = await _userManager.FindByNameAsync(userNameAsString);

            if (user == null)
            {
                return;
            }

            bindingContext.Result = ModelBindingResult.Success(user);
        }
    }
}
