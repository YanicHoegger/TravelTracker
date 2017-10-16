using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TravelTracker.User;
using Xunit;

namespace UnitTests.LoginControllerTests
{
    public class LoginViewModelTests : ViewModelTestBase
    {
        [Fact]
        public void EmptyPropertiesTest()
        {
            GivenViewModel(null, null);

            WhenValidate();

            ThenInvalid("The Email field is required.", "The Password field is required.");
        }

        [Fact]
        public void ValidPropertiesTest()
        {
            GivenViewModel("something", "something");

            WhenValidate();

            ThenValid();
        }

        LoginViewModel viewModel;

        protected override object ViewModel => viewModel;

        void GivenViewModel(string email, string password)
        {
            viewModel = new LoginViewModel()
            {
                Email = email,
                Password = password
            };
        }
    }
}
