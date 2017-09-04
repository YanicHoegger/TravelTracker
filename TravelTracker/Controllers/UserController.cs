using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.User;

namespace TravelTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user)
        {
            if(user == null)
            {
                return NotFound();   
            }

            return View(user);
        }

        [Authorize(Policy = "UserLogedIn")]
        public async Task<IActionResult> ChangeUserName([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, string newUserName)
        {
            if(user == null)
            {
                return NotFound();
            }

            //if(_signInManager.IsSignedIn(HttpContext.User) && HttpContext.User.Identity.Name == user.UserName)
            //{
            //}

            user.UserName = newUserName;

            await _userManager.UpdateAsync(user);

            return Redirect("~/" + newUserName);
        }
    }
}
