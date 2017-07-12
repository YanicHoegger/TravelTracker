using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace TravelTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string repassword)
        {
            if (password != repassword)
            {
                ModelState.AddModelError(string.Empty, "Password don't match");
                return View();
            }

            var newUser = new IdentityUser 
            {
                UserName = email,
                Email = email
            };

            var userCreationResult = await _userManager.CreateAsync(newUser, password);
            if (!userCreationResult.Succeeded)
            {
                foreach(var error in userCreationResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View();
            }

            return Content("Registering user successful");
        }
    }
}