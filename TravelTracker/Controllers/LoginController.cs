using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.User;

namespace TravelTracker.Controllers
{
    public class LoginController : Controller
    {
		readonly UserManager<IdentityUser> _userManager;
		readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //TODO:Check if https is needed
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
		{
			if(!TryValidateModel(viewModel))
            {
                return ViewComponent("LoginStatus", viewModel);
			}

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
			if (user == null)
			{
				ModelState.AddModelError("Password", "Invalid login");
                return ViewComponent("LoginStatus", viewModel);
			}

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false);
			if (!passwordSignInResult.Succeeded)
			{
				ModelState.AddModelError("Password", "Invalid login"); 
                return ViewComponent("LoginStatus", viewModel);
			}

            var userUrlLink = Url.Link("users", new { username = user.UserName, action = "" });
            return Content($"Redirect {userUrlLink}");
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			//TODO: At the moment the landing page is the only page a user hasn't to be logedin,
            //but as soon there are other pages a user doesn't has to be logedin,
            //the user should stay on those pages
			return Redirect("~/"); 
		}
    }
}
