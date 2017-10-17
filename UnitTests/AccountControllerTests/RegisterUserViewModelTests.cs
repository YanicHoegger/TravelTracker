﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TravelTracker.User;
using Xunit;

namespace UnitTests.AccountControllerTests
{
    public class RegisterUserViewModelTests : ViewModelTestBase
    {
        [Fact]
        public void ViewModelValidWhenValidPropertiesTest()
        {
            GivenModel();

            WhenValidate();

            ThenValid();
        }

        [Fact]
        public void ViewModelInvalidWhenEmptyPropertiesTest()
        {
            GivenModel(null, null, null, null);

            WhenValidate();

            ThenInvalid("The Email field is required.",
                       "The User Name field is required",
                       "The Password field is required.",
                       "The Retype Password field is required");
        }

        //TODO; Check list from wikipedia
        [Theory]
        [InlineData("noAt.ch")]
        [InlineData("nothingAfter@")]
        public void WrongEmailViewModelInvalidTest(string email)
        {
            GivenModel(email);

            WhenValidate();

            ThenInvalid("The Email field is not a valid e-mail address.");
        }

        [Fact]
        public void PasswordAndRepasswordNotEqualViewModelInvalidTest()
        {
            GivenModel(password: "SomethingValid12", retypePassword: "SomethingValidButDifferent12");

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
            GivenModel(password: password, retypePassword: password);

            WhenValidate();

            ThenInvalid("The Email field is not a valid e-mail address.");
        }

        RegisterUserViewModel model;

        protected override object ViewModel => model;

        void GivenModel(string email = "right@email.com", string userName = "userName", string password = "123AAAaaa", string retypePassword = "123AAAaaa")
        {
            model = new RegisterUserViewModel()
            {
                Email = email,
                UserName = userName,
                Password = password,
                RetypePassword = retypePassword
            };
        }
    }
}