using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using TravelTracker.Controllers;
using TravelTracker.Messages;

namespace UnitTests
{
    public class AccountControllerMock
    {
        private readonly AccountController _controller;
        private readonly MessageCollection _messageCollection = new MessageCollection();

        public AccountControllerMock()
        {
            //TODO: Check if really neaded
			var users = new List<IdentityUser>
				{
					new IdentityUser
					{
						UserName = "Test",
						Id = Guid.NewGuid().ToString(),
						Email = "test@test.it"
					}

				}.AsQueryable();

			var fakeUserManager = new Mock<UserManagerMock>();

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

			var signInManager = new Mock<SignInManagerMock>();

			signInManager.Setup(
					x => x.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.Success);


			//SERVICES CONFIGURATIONS
			_controller = new AccountController(fakeUserManager.Object, signInManager.Object, _messageCollection);
        }

        public AccountController AccountController
        {
            get 
            {
                return _controller;
            }
        }

        public MessageCollection MessageCollection 
        {
            get 
            {
                return _messageCollection;
            }
        }
    }
}
