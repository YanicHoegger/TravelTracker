using System;
using TravelTracker.User;
using Xunit;

namespace UnitTests.UserControllerTests
{
    public class NewUserNameViewModelTests : ViewModelTestBase
    {
        [Fact]
        public void EmptyPropertyTest()
        {
            GivenViewModel(null);

            WhenValidate();

            ThenInvalid("The New User Name field is required");
        }

        [Fact]
        public void ValidViewModelTest()
        {
            GivenViewModel("username");

            WhenValidate();

            ThenValid();
        }

        NewUserNameViewModel viewModel;

        protected override object ViewModel => viewModel;

		void GivenViewModel(string newUserName)
		{
            viewModel = new NewUserNameViewModel()
            {
                NewUserName = newUserName
            };
		}
    }
}
