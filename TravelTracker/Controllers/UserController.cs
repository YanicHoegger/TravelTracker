using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.User;

namespace TravelTracker.Controllers
{
    //TODO: Feedback if not successfull changed data
    [Authorize(Policy = "UserLogedIn")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
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

        [HttpPost]
        public async Task<IActionResult> ChangeUserName([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, string newUserName)
        {
            if(user == null)
            {
                return NotFound();
            }

            user.UserName = newUserName;

            await _userManager.UpdateAsync(user);

            return Redirect("~/" + newUserName);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, string newEmail)
        {
			if (user == null)
			{
				return NotFound();
			}

            user.Email = newEmail;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
		public async Task<IActionResult> ChangePassword([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, 
                                                        string currentPassword, string newPassword, string retypeNewPassword)
		{
			if (user == null)
			{
				return NotFound();
			}

            if(!newPassword.Equals(retypeNewPassword))
            {
                return RedirectToAction(nameof(Index));
            }

            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

			return RedirectToAction(nameof(Index));
		}
    }
}
