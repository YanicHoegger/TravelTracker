using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.User;

namespace TravelTracker.Controllers
{
	//TODO: Instead of Access Denied Page, 'localhost redirected you too many times' comes
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
            var viewModel = new DisplayAllViewModel();
            viewModel.Users = _userManager.Users.Select(CreateUserViewModel);
            return View(viewModel);
        }

        private UserViewModel CreateUserViewModel(IdentityUser user)
        {
            var viewModel = new UserViewModel();
            viewModel.UserName = user.UserName;
            return viewModel;
        }
	}
}