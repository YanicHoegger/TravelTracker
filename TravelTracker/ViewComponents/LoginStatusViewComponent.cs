using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace TravelTracker
{
    public class LoginStatusViewComponent : ViewComponent
	{
		readonly UserManager<IdentityUser> _userManager;
		readonly SignInManager<IdentityUser> _signInManager;
		readonly IUrlHelperFactory _factory;

		public LoginStatusViewComponent(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
            //var helper = _factory.GetUrlHelper(ViewContext);
            //helper.ActionContext.RouteData.
            //return null;

            if(_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                return View("LoggedIn", user);
            }
            else
            {
                return View();
            }
		}
    }
}
