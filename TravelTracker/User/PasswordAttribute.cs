using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelTracker.User
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordAttribute : ValidationAttribute, IClientModelValidator
    {
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

            var identityOptionsProvider = (IIdentityOptionsProvider)context.ActionContext.HttpContext.RequestServices.GetService(typeof(IIdentityOptionsProvider));

            if (identityOptionsProvider == null)
            {
                throw new ServiceCollectionException($"Can't find '{nameof(IIdentityOptionsProvider)}' service in service collection");
            }

			MergeAttribute(context.Attributes, "data-val", "true");

            if(identityOptionsProvider.PasswordRequireDigit)
            {
                MergeAttribute(context.Attributes, "data-val-digit", "Password must contain a digit");
            }

            MergeAttribute(context.Attributes, "data-val-passwordlength", $"Password must be at least {identityOptionsProvider.PasswordRequiredLength} characters long");
            MergeAttribute(context.Attributes, "data-val-passwordlength-length", identityOptionsProvider.PasswordRequiredLength.ToString());

            if(identityOptionsProvider.PasswordRequireNonAlphanumeric)
            {
				MergeAttribute(context.Attributes, "data-val-nonalphanumeric", "Password must contain a non alphanumeric character");
            }
           
            if(identityOptionsProvider.PasswordRequireUppercase)
            {
                MergeAttribute(context.Attributes, "data-val-uppercase", "Password must contain am upper case character");
            }

            if(identityOptionsProvider.PasswordRequireLowercase)
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
