using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TravelTracker.User
{
    public class UserDetailsViewModel
    {
        public UserDetailsViewModel() { }

        public UserDetailsViewModel(IdentityUser identityUser)
        {
            UpdateFromIdentityUser(this, identityUser);
        }

        public NewEmailViewModel NewEmail { get; set; }

        public NewUserNameViewModel NewUserName { get; set; }

        public NewPasswordViewModel NewPassword { get; set; }

        public void UpdateFromIdentityUser(IdentityUser identityUser)
        {
            UpdateFromIdentityUser(this, identityUser);
        }

        static void UpdateFromIdentityUser(UserDetailsViewModel model, IdentityUser identityUser)
        {
            model.NewUserName = new NewUserNameViewModel(identityUser.UserName);
            model.NewEmail = new NewEmailViewModel(identityUser.Email);
        }
    }

    public class NewUserNameViewModel
    {
        public NewUserNameViewModel() { }

        public NewUserNameViewModel(string userName)
        {
            NewUserName = userName;
        }

        [Required(ErrorMessage = "The New User Name field is required")]
        public string NewUserName { get; set; }
    }

    public class NewEmailViewModel
    {
        public NewEmailViewModel() { }

        public NewEmailViewModel(string email)
        {
            NewEmail = email;
        }

        [Required(ErrorMessage = "The New Email field is required")]
        [EmailAddress(ErrorMessage = "This is not a valid e-mail address")]
		public string NewEmail { get; set; }
    }

    public class NewPasswordViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "The Current Password field is required")]
		public string CurrentPassword { get; set; }

		[Required(ErrorMessage = "The New Password field is required")]
        [Password]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "The Retype New Password field is required")]
        [Password]
		public string RetypeNewPassword { get; set; }

		//Field only be used to show Errors that can either come from CurrentPassword or from NewPassword
		public string PasswordError { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (!NewPassword.Equals(RetypeNewPassword))
			{
				yield return new ValidationResult("Passwords don't match", new[] { "NewPassword", "RetypeNewPassword" });
			}
		}
    }
}
