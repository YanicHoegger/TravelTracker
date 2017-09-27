using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelTracker.User
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordAttribute : ValidationAttribute, IClientModelValidator
    {
        readonly IIdentityOptionsProvider _identityOptionsProvider;

        public PasswordAttribute() : this(new IdentityOptionsProvider()) { }

        public PasswordAttribute(IIdentityOptionsProvider identityOptionsProvider)
        {
            _identityOptionsProvider = identityOptionsProvider;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
            //Attribute is only used for client side validation
            //Server side validation is already implemented in UserManager
            return ValidationResult.Success;
		}

        public void AddValidation(ClientModelValidationContext context)
        {
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			MergeAttribute(context.Attributes, "data-val", "true");

            if(_identityOptionsProvider.PasswordRequireDigit)
            {
                MergeAttribute(context.Attributes, "data-val-digit", "Password must contain a digit");
            }

            MergeAttribute(context.Attributes, "data-val-passwordlength", $"Password must be at least {_identityOptionsProvider.PasswordRequiredLength} characters long");
            MergeAttribute(context.Attributes, "data-val-passwordlength-length", _identityOptionsProvider.PasswordRequiredLength.ToString());

            if(_identityOptionsProvider.PasswordRequireNonAlphanumeric)
            {
				MergeAttribute(context.Attributes, "data-val-nonalphanumeric", "Password must contain a non alphanumeric character");
            }
           
            if(_identityOptionsProvider.PasswordRequireUppercase)
            {
                MergeAttribute(context.Attributes, "data-val-uppercase", "Password must contain am upper case character");
            }

            if(_identityOptionsProvider.PasswordRequireLowercase)
            {
                MergeAttribute(context.Attributes, "data-val-lowercase", "Password must contain a lower case character");
            }
        }

        bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }
}
