using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelTracker.User
{
    public class RegisterUserViewModel : IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The User Name field is required")]
        public string UserName { get; set; }

        [Required]
        [Password]
        public string Password { get; set; }

        [Required(ErrorMessage = "The Retype Password field is required")]
        [Password]
        public string RetypePassword { get; set; }

        //Field only used for error messages that can come from any field
        public string ErrorMessage { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!Password.Equals(RetypePassword))
            {
                yield return new ValidationResult("Passwords don't match");
            }
        }
    }
}
;