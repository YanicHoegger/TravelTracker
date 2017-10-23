using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace TravelTracker.User
{
    public class IdentityOptionsProvider : IIdentityOptionsProvider
    {
        public IdentityOptionsProvider(IConfigurationRoot config)
        {
            PasswordRequireDigit = Convert.ToBoolean(config["Identity:RequireDigit"]);
            PasswordRequiredLength = Convert.ToInt32(config["Identity:RequiredLength"]);
            PasswordRequireNonAlphanumeric = Convert.ToBoolean(config["Identity:RequireNonAlphanumeric"]);
            PasswordRequireUppercase = Convert.ToBoolean(config["Identity:RequireUppercase"]);
            PasswordRequireLowercase = Convert.ToBoolean(config["Identity:RequireLowercase"]);
            RequireDigitUniqueEmail = Convert.ToBoolean(config["Identity:RequireUniqueEmail"]);
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
    }
}
