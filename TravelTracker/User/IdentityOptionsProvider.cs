using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace TravelTracker.User
{
    public class IdentityOptionsProvider : IIdentityOptionsProvider
    {
        public IdentityOptionsProvider(IConfigurationRoot config)
        {
            PasswordRequireDigit = Convert.ToBoolean(GetConfig(config, "Identity:Password:RequireDigit"));
            PasswordRequiredLength = Convert.ToInt32(GetConfig(config, "Identity:Password:RequiredLength"));
            PasswordRequireNonAlphanumeric = Convert.ToBoolean(GetConfig(config, "Identity:Password:RequireNonAlphanumeric"));
            PasswordRequireUppercase = Convert.ToBoolean(GetConfig(config, "Identity:Password:RequireUppercase"));
            PasswordRequireLowercase = Convert.ToBoolean(GetConfig(config, "Identity:Password:RequireLowercase"));
            RequireDigitUniqueEmail = Convert.ToBoolean(GetConfig(config, "Identity:RequireUniqueEmail"));
        }

        public void SetOptions(IdentityOptions options)
        {
			options.Password.RequireDigit = PasswordRequireDigit;
			options.Password.RequiredLength = PasswordRequiredLength;
			options.Password.RequireNonAlphanumeric = PasswordRequireNonAlphanumeric;
			options.Password.RequireUppercase = PasswordRequireUppercase;
			options.Password.RequireLowercase = PasswordRequireLowercase;
			options.User.RequireUniqueEmail = RequireDigitUniqueEmail;
        }

        public bool PasswordRequireDigit { get; }
        public int PasswordRequiredLength { get; }
        public bool PasswordRequireNonAlphanumeric { get; }
        public bool PasswordRequireUppercase { get; }
        public bool PasswordRequireLowercase { get; }
        public bool RequireDigitUniqueEmail { get; }

        static string GetConfig(IConfigurationRoot config, string selector)
        {
            var result = config[selector];

            if(result == null)
            {
                throw new ArgumentException($"There is no setting for '{selector}'");
            }

            return result;
        }
    }
}
