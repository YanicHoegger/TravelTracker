using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelTracker.User
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        readonly IdentityOptionsProvider identityOptionsProvider = new IdentityOptionsProvider();

        public void AddValidation(ClientModelValidationContext context)
        {
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			MergeAttribute(context.Attributes, "data-val", "true");

            MergeAttribute(context.Attributes, "data-val-require-digit", identityOptionsProvider.PasswordRequireDigit.ToString());
            MergeAttribute(context.Attributes, "data-val-passwordlength", $"Password must be at least {identityOptionsProvider.PasswordRequiredLength} characters long");
            MergeAttribute(context.Attributes, "data-val-passwordlength-length", identityOptionsProvider.PasswordRequiredLength.ToString());
            MergeAttribute(context.Attributes, "data-val-require-non-alphanumeric", identityOptionsProvider.PasswordRequireNonAlphanumeric.ToString());
            if(identityOptionsProvider.PasswordRequireUppercase)
            {
                MergeAttribute(context.Attributes, "data-val-uppercase", "Password must contain upper case characters");
            }
            MergeAttribute(context.Attributes, "data-val-require-lower-case", identityOptionsProvider.PasswordRequireLowercase.ToString());
        }

		private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
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
