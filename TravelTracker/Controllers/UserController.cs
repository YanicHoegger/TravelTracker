using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.User;

namespace TravelTracker.Controllers
{
    //TODO: Instead of Access Denied Page, 'localhost redirected you too many times' comes
    [Authorize(Policy = "UserLogedIn")]
    public class UserController : Controller
    {
        readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user)
        {
            if (user == null)
            {
                return NotFound();
            }

            return View(CreateViewModel(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserName([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, 
                                                        UserDetailsViewModel viewModel)
        {
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                //No need to update the whole view model, because in error case only new user name will be visible
                return View(nameof(Index), viewModel);
            }

            user.UserName = viewModel.NewUserName.Value;
            var identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
				foreach (var error in identityResult.Errors)
				{
					ModelState.AddModelError("NewUserName.NewUserName", error.Description);
				}

				//No need to update the whole view model, because in error case only new user name will be visible
				return View(nameof(Index), viewModel);
            }

            return Redirect(Url.RouteUrl("users", new { username = user.UserName, action = "" }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, UserDetailsViewModel viewModel)
        {
            if (user == null)
            {
                return NotFound();
            }

			if (!ModelState.IsValid)
			{
				//No need to update the whole view model, because in error case only new user name will be visible
				return View(nameof(Index), viewModel);
			}

            user.Email = viewModel.NewEmail.Value;
			var identityResult = await _userManager.UpdateAsync(user);

            if(!identityResult.Succeeded)
            {
				foreach (var error in identityResult.Errors)
				{
					ModelState.AddModelError("NewEmail.NewEmail", error.Description);
				}

                return View(nameof(Index), viewModel);
            }

            return View(nameof(Index), CreateViewModel(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, UserDetailsViewModel viewModel)
        {
            if (user == null)
            {
                return NotFound();
            }

			if (!ModelState.IsValid)
			{
				//No need to update the whole view model, because in error case only new user name will be visible
				return View(nameof(Index), viewModel);
			}

            var identityResult = await _userManager.ChangePasswordAsync(user, viewModel.NewPassword.CurrentPassword, viewModel.NewPassword.NewPassword);

            if(!identityResult.Succeeded)
            {
				foreach (var error in identityResult.Errors)
				{
					ModelState.AddModelError("NewPassword.PasswordError", error.Description);
				}

				return View(nameof(Index), viewModel); 
            }

            return View(nameof(Index), CreateViewModel(user));
        }

        UserDetailsViewModel CreateViewModel(IdentityUser user)
        {
            return new UserDetailsViewModel()
            {
                NewEmail = new NewEmailViewModel()
                {
                    Value = user.Email
                },
                NewUserName = new NewUserNameViewModel()
                {
                    Value = user.UserName
                },
                NewPassword = new NewPasswordViewModel()
            };
        }
    }
}
