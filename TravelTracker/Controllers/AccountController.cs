using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.Messages;

namespace TravelTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMessageCollection _messageCollection;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMessageCollection messageCollection)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageCollection = messageCollection;
        }

        public IActionResult Register()
        {
            return View();
        }    
        
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string repassword)
        {
            var isValidInput = true;
            if (string.IsNullOrEmpty(email))
            {
                _messageCollection.Add(new ErrorInFieldMessage("Email is empty"));
                isValidInput = false;
            }
            if (string.IsNullOrEmpty(password))
            {
                _messageCollection.Add(new ErrorInFieldMessage("Password is empty"));
                isValidInput = false;
            }
            if (string.IsNullOrEmpty(repassword))
            {
                _messageCollection.Add(new ErrorInFieldMessage("Retype password is empty"));
                isValidInput = false;
            }
            if (password != repassword)
            {
                _messageCollection.Add(new ErrorInFieldMessage("Passwords don't match"));   
                isValidInput = false;
            }

            if (!isValidInput)
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
                {
                    _messageCollection.Add(new ErrorInFieldMessage(error.Description));
                }
                    
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
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        } 
    }
}