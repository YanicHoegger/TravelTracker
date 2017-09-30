using System;
namespace TravelTracker.User
{
    public interface IIdentityOptionsProvider
    {
        bool PasswordRequireDigit { get; }
		int PasswordRequiredLength { get; }
		bool PasswordRequireNonAlphanumeric { get; }
		bool PasswordRequireUppercase { get; }
		bool PasswordRequireLowercase { get; }
		bool RequireDigitUniqueEmail { get; }
    }
}
