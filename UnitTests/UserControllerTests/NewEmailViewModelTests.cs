using System;
using TravelTracker.User;
using Xunit;

namespace UnitTests.UserControllerTests
{
    public class NewEmailViewModelTests : ViewModelTestBase
    {
        [Fact]
        public void EmptyPropertiesTest()
        {
            GivenViewModel(null);

            WhenValidate();

            ThenInvalid("The New Email field is required");
        }

        [Theory]
        [InlineData("somethingWithout.at")]
        [InlineData("something@")]
        public void WrongEmailTest(string email)
        {
            GivenViewModel(email);

			WhenValidate();

            ThenInvalid("This is not a valid e-mail address");
        }

        [Fact]
        public void RightEmailTest()
        {
            GivenViewModel("somethin@right.com");

            WhenValidate();

            ThenValid();
        }

        NewEmailViewModel viewModel;

        protected override object ViewModel => viewModel;

        void GivenViewModel(string email)
        {
            viewModel = new NewEmailViewModel()
            {
                Value = email
            };
        }
    }
}
