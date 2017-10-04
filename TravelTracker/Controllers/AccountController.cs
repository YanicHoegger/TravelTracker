using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.User;

namespace TravelTracker.Controllers
{
	//TODO: Instead of Access Denied Page, 'localhost redirectes page can’t be found
	[Authorize(Roles = "Administrator")]
    public class AccountController : Controller
    {
        readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel viewModel)
        {
            if (!TryValidateModel(viewModel))
            {
                return View(viewModel);
            }

            var newUser = new IdentityUser
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email
            };

            var userCreationResult = await _userManager.CreateAsync(newUser, viewModel.Password);
            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    ModelState.AddModelError("ErrorMessage", error.Description);
                }

                return View(viewModel);
            }

            //TODO: Remove as soon as a better way is found to give admin rights
            //await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, "Administrator"));

            return Content("Registering user successful");
        }

        public IActionResult DisplayAll()
        {
            return View(_userManager.Users.Select(CreateUserViewModel));
        }

        UserViewModel CreateUserViewModel(IdentityUser user)
        {
            var viewModel = new UserViewModel();

            viewModel.UserName = user.UserName;

            return viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            await _userManager.DeleteAsync(user);

            return Redirect(nameof(DisplayAll));
        }
	}
}