﻿using System.Threading.Tasks;
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

            return View(new UserDetailsViewModel(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserName([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, UserDetailsViewModel viewModel)
        {
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = viewModel.NewUserName.NewUserName;

            viewModel.UpdateFromIdentityUser(user);

            if (!TryValidateModel(viewModel.NewUserName))
            {
                return View(nameof(Index), viewModel);
            }

            var identityResult = await _userManager.UpdateAsync(user);

            if (identityResult.Succeeded)
            {
                return Redirect("~/" + user.UserName);
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("NewUserName.NewUserName", error.Description);
            }

            return View(nameof(Index), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, UserDetailsViewModel viewModel)
        {
            if (user == null)
            {
                return NotFound();
            }

            user.Email = viewModel.NewEmail.NewEmail;

            viewModel.UpdateFromIdentityUser(user);

            //Validation for correct email is in ViewModel, validation for 'email is not used already' is in UserManager
            if (TryValidateModel(viewModel.NewEmail))
            {
                var identityResult = await _userManager.UpdateAsync(user);

                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("NewEmail.NewEmail", error.Description);
                }
            }

            return View(nameof(Index), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user, UserDetailsViewModel viewModel)
        {
            if (user == null)
            {
                return NotFound();
            }

            viewModel.UpdateFromIdentityUser(user);

            if (!TryValidateModel(viewModel))
            {
                return View(nameof(Index), viewModel);
            }

            var identityResult = await _userManager.ChangePasswordAsync(user, viewModel.NewPassword.CurrentPassword, viewModel.NewPassword.NewPassword);

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("NewPassword.PasswordError", error.Description);
            }

            return View(nameof(Index), viewModel);
        }

        //TODO: Remove as soon as a better way is found to give admin rights
        //public async Task<IActionResult> MakeAdmin([ModelBinder(BinderType = typeof(UserBinder))] IdentityUser user)
        //{
        //    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Administrator"));

        //    return Redirect("~/");
        //}
    }
}
