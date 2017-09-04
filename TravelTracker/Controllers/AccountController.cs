using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.Messages;

namespace TravelTracker.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;
        readonly IMessageCollection _messageCollection;

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

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Register(string email, string userName, string password, string repassword)
        {
            if (!CheckInput(email, userName, password, repassword))
            {
                return View();
            }

            var newUser = new IdentityUser
            {
                UserName = userName,
                Email = email
            };

            var userCreationResult = await _userManager.CreateAsync(newUser, password);
            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    _messageCollection.Add(new ErrorInFieldMessage(error.Description));
                }

                return View();
            }

            //TODO: Remove as soon as a better way is found to give admin rights
            //await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, "Administrator"));

            return Content("Registering user successful");
        }

        private bool CheckInput(string email, string userName, string password, string repassword)
        {
            var isEmailValid = CheckInputIfNullOrEmpty(email, "Email is empty");
            var isUserNameValid = CheckInputIfNullOrEmpty(userName, "User Name is empty");
            var isPasswordValid = CheckInputIfNullOrEmpty(password, "Password is empty");

            //Repassword needs no Null or empty check, because it will be checked anyway when checked for equality with password

            var isPasswordAndRepasswordEqual = true;
            if (isPasswordValid && !password.Equals(repassword))
            {
                _messageCollection.Add(new ErrorInFieldMessage("Passwords don't match"));
                isPasswordAndRepasswordEqual = false;
            }

            return isEmailValid && isUserNameValid && isPasswordValid && isPasswordAndRepasswordEqual;
        }

        bool CheckInputIfNullOrEmpty(string toCheck, string errorMessage)
        {
            if (string.IsNullOrEmpty(toCheck))
            {
                _messageCollection.Add(new ErrorInFieldMessage(errorMessage));
                return false;
            }
            return true;
        }

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
            return Redirect("~/");
        }
    }
}