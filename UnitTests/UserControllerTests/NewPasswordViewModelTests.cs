using System;
using TravelTracker.User;
using Xunit;

namespace UnitTests.UserControllerTests
{
    public class NewPasswordViewModelTests : ViewModelTestBase
    {
        [Fact]
        public void EmptyPropertiesTest()
        {
            GivenViewModel(null, null, null);

            WhenValidate();

            ThenInvalid("The Current Password field is required", "The New Password field is required", "The Retype New Password field is required");
        }

		[Fact]
		public void PasswordAndRepasswordNotEqualViewModelInvalidTest()
        {
            GivenViewModel(newPassword: "something", retypeNewPassword: "somethingDifferent");

            WhenValidate();

            ThenInvalid("Passwords don't match");
        }


		[Theory(Skip = "Is handled by the user Manager")]
		[InlineData("noNumber")]
		[InlineData("noupeercase1234")]
		[InlineData("NOLOWERCASE1234")]
		[InlineData("Aa123")]
		public void WrongPasswordViewModelInvalidTest(string password)
		{
            GivenViewModel(newPassword: password, retypeNewPassword: password);

            WhenValidate();

            ThenInvalid("SomeError");
		}

        [Fact]
        public void ValidViewModelTest()
        {
            GivenViewModel();

            WhenValidate();

            ThenValid();
        }

        NewPasswordViewModel viewModel;

        protected override object ViewModel => viewModel;

        void GivenViewModel(string currentPassword  = "something", string newPassword = "something", string retypeNewPassword = "something")
        {
            viewModel = new NewPasswordViewModel()
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
                RetypeNewPassword = retypeNewPassword
            };
        }
    }
}
