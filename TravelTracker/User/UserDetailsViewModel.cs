using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TravelTracker.User
{
    public class UserDetailsViewModel
    {
        public NewEmailViewModel NewEmail { get; set; }

        public NewUserNameViewModel NewUserName { get; set; }

        public NewPasswordViewModel NewPassword { get; set; }
    }

    public class NewUserNameViewModel
    {
        [Required(ErrorMessage = "The New User Name field is required")]
        public string Value { get; set; }
    }

    public class NewEmailViewModel
    {
        [Required(ErrorMessage = "The New Email field is required")]
        [EmailAddress(ErrorMessage = "This is not a valid e-mail address")]
		public string Value { get; set; }
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
            if(NewPassword != null)
            {
				if (!NewPassword.Equals(RetypeNewPassword))
				{
					yield return new ValidationResult("Passwords don't match", new[] { "NewPassword", "RetypeNewPassword" });
				}
            }
		}
    }
}
