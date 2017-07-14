using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.Controllers;
using Xunit;
using Moq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace UnitTests
{
    public class AccountControllerTests
    {
        public AccountControllerTests()
        {
            var users = new List<IdentityUser>
            {
                new IdentityUser
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.it"
                }

            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();

            fakeUserManager.Setup(x => x.Users)
                .Returns(users);

            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var uservalidator = new Mock<IUserValidator<IdentityUser>>();
            uservalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<IdentityUser>>(), It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);
            var passwordvalidator = new Mock<IPasswordValidator<IdentityUser>>();
            passwordvalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<IdentityUser>>(), It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var signInManager = new Mock<FakeSignInManager>();

            signInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);


            //SERVICES CONFIGURATIONS
            _controller = new AccountController(fakeUserManager.Object, signInManager.Object);
        }
        
        [Fact]
        public void RegisterProduceErrorMessageWhenEmptyFields()
        {
            GivenEmptyFields();

            WhenRegister();

            ThenResultViewWithEmptyFieldsMessage();
        }

        [Fact]
        public void RegisterSuccessfulWhenCorrectFields()
        {
            GivenCorrectFields();
            
            WhenRegister();

            ThenResultViewIsSuccessView();
        }


        private readonly AccountController _controller;
        private string _emailAddress;
        private string _password;
        private string _repassword;
        private IActionResult _result;

        private void GivenEmptyFields()
        {
            _emailAddress = null;
            _password = null;
            _repassword = null;
        }

        private void GivenCorrectFields()
        {
            _emailAddress = "test@test.ch";
            _password = "AsDfg";
            _repassword = "AsDfg";
        }

        private void WhenRegister()
        {
            _result = _controller.Register(_emailAddress, _password, _repassword).Result;
        }

        private void ThenResultViewWithEmptyFieldsMessage()
        {
            var resultAsViewResult = Assert.IsType<ViewResult>(_result);

            Assert.Equal(3, resultAsViewResult.ViewData.ModelState.ErrorCount);
        }

        private void ThenResultViewIsSuccessView()
        {
            var resultAsContentResult = Assert.IsType<ContentResult>(_result);
            
            Assert.Equal("Registering user successful", resultAsContentResult.Content);
        }

    }
}