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

  //      [HttpPost]
  //      public async Task<IActionResult> Login(LoginViewModel viewModel)
		//{
  //          if(!TryValidateModel(viewModel))
  //          {
  //              return Redirect("~/"); //TODO: Check the correct way to stay on the same page
		//	}

  //          var user = await _userManager.FindByEmailAsync(viewModel.Email);
		//	if (user == null)
		//	{
		//		ModelState.AddModelError("Email", "Invalid login");
  //              return Redirect("~/"); //TODO: Check the correct way to stay on the same page
		//	}

  //          var passwordSignInResult = await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false);
		//	if (!passwordSignInResult.Succeeded)
		//	{
		//		ModelState.AddModelError("Password", "Invalid login"); 
		//		return Redirect("~/"); //TODO: Check the correct way to stay on the same page
		//	}

		//	return Redirect("~/" + user.UserName);
		//}

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email ?? ""); //TODO: Is there a better way, so the arguments would come as empty strings instead of null???
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                //return View();
                return Redirect("~/");
            }

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(user, password ?? "", rememberMe, false);
            if (!passwordSignInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                //return View();  
                return Redirect("~/");
            }

            return Redirect("~/" + user.UserName);
        }

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Redirect("~/"); //TODO: Check the correct way to stay on the same page
		}
    }
}
