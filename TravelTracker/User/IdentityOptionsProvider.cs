using Microsoft.AspNetCore.Builder;

namespace TravelTracker.User
{
    public class IdentityOptionsProvider : IIdentityOptionsProvider
    {
		readonly bool _passwordRequireDigit = false;
		readonly int _passwordRequiredLength = 6;
		readonly bool _passwordRequireNonAlphanumeric = false;
		readonly bool _passwordRequireUppercase = true;
		readonly bool _passwordRequireLowercase = true;
		readonly bool _RequireDigitUniqueEmail = true;

        public void SetOptions(IdentityOptions options)
        {
			options.Password.RequireDigit = PasswordRequireDigit;
			options.Password.RequiredLength = PasswordRequiredLength;
			options.Password.RequireNonAlphanumeric = PasswordRequireNonAlphanumeric;
			options.Password.RequireUppercase = PasswordRequireUppercase;
			options.Password.RequireLowercase = PasswordRequireLowercase;
			options.User.RequireUniqueEmail = RequireDigitUniqueEmail;
        }

		public bool PasswordRequireDigit => _passwordRequireDigit;
		public int PasswordRequiredLength => _passwordRequiredLength;
		public bool PasswordRequireNonAlphanumeric => _passwordRequireNonAlphanumeric;
		public bool PasswordRequireUppercase => _passwordRequireUppercase;
		public bool PasswordRequireLowercase => _passwordRequireLowercase;
		public bool RequireDigitUniqueEmail => _RequireDigitUniqueEmail;
    }
}
