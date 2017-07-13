using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Email is empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Password is empty");
            }
            if (string.IsNullOrEmpty(repassword))
            {
                ModelState.AddModelError(string.Empty, "Retype password is empty");
            }
            if (password != repassword)
            {
                ModelState.AddModelError(string.Empty, "Password don't match");        
            }

            if (!ModelState.IsValid)
            {
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
        
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                //return View();
                return Redirect("~/");
            }

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
            if (!passwordSignInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                //return View();  
                return Redirect("~/");
            }

            return Redirect("~/");
        }
    }
}