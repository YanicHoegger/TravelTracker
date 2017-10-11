using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.User;

namespace TravelTracker.ViewComponents
{
    public class LoginStatusViewComponent : ViewComponent
	{
		readonly UserManager<IdentityUser> _userManager;
		readonly SignInManager<IdentityUser> _signInManager;

		public LoginStatusViewComponent(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
            if(_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var viewModel = new UserViewModel()
                {
                    UserName = user.UserName,
                    RouteUrl = Url.RouteUrl("users", new { username = user.UserName, action = "" })
                };

                return View("LoggedIn", viewModel);
            }
            else
            {
                return View();
            }
		}
    }
}
