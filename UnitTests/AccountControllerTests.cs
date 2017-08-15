using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace UnitTests
{    
    public class AccountControllerTests
    {   
        
        [Theory]
		[InlineData(FieldEnum.EmailAddress, "Email is empty")]
		[InlineData(FieldEnum.Password, "Password is empty")]
		[InlineData(FieldEnum.Repassword, "Retype password is empty")]
        public void RegisterProduceErrorMessageWhenEmptyFields(FieldEnum fieldEnum, string incorrectMessage)
        {
            GivenCorrectFields();
            GivenEmptyField(fieldEnum);

            WhenRegister();

            ThenResultViewWithIncorrectFieldMessage(incorrectMessage);
        }

        [Fact]
        public void RegisterSuccessfulWhenCorrectFields()
        {
            GivenCorrectFields();
            
            WhenRegister();

            ThenResultViewIsSuccessView();
        }

        //TODO: When have a better solution for genarating unique messages, enable tests again
        [Theory(Skip = "Is handled by the user Manager")]
        [InlineData(FieldEnum.EmailAddress, "somethingWithoutAnAt", "")]
        [InlineData(FieldEnum.Password, "asd", "")]
        public void RegisterProduceErrorMessageWhenIncorrectFields(FieldEnum fieldEnum, string incorrectValue, string incorrectMessage)
        {
            GivenCorrectFields();
            GivenIncorrectField(fieldEnum, incorrectValue);

            WhenRegister();
            
            ThenResultViewWithIncorrectFieldMessage(incorrectMessage);
        }

        [Fact]
        public void RegisterDifferentPasswordAndRepasswordProduceErrorMessage()
        {
            GivenDifferentPasswordAndRepassword();
            
            WhenRegister();
            
            ThenResultViewWithIncorrectFieldMessage("Passwords don't match");
        }

        #region

        private readonly AccountControllerMock _accountControllerMock = new AccountControllerMock();
        private string _emailAddress;
        private string _password;
        private string _repassword;
        private IActionResult _result;

        private void GivenEmptyField(FieldEnum fieldEnum)
        {
            SetFieldValue(fieldEnum, null);
        }

        private void GivenCorrectFields()
        {
            _emailAddress = "test@test.ch";
            _password = "AsDfg";
            _repassword = "AsDfg";
        }

        private void GivenIncorrectField(FieldEnum field, string incorrectFieldValue)
        {
            SetFieldValue(field, incorrectFieldValue);

            //Password and repassword have to be the same for this test, 
            //otherwise an error for not equal password and repassword would occour first
            if (field.Equals(FieldEnum.Password))
            {
                SetFieldValue(FieldEnum.Repassword, incorrectFieldValue);
            }

			//Password and repassword have to be the same for this test, 
			//otherwise an error for not equal password and repassword would occour first
            if (field.Equals(FieldEnum.Repassword))
			{
                SetFieldValue(FieldEnum.Password, incorrectFieldValue);
			}
        }

        private void SetFieldValue(FieldEnum field, string fieldValue)
        {
			switch (field)
			{
				case FieldEnum.EmailAddress:
					_emailAddress = fieldValue;
					break;
				case FieldEnum.Password:
					_password = fieldValue;
					break;
				case FieldEnum.Repassword:
					_repassword = fieldValue;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(field), field, null);
			}
        }

        private void GivenDifferentPasswordAndRepassword()
        {
            _password = "SomethingCorrect";
            _repassword = "SomethingCorrectButDifferent";
            _emailAddress = null;
        }

        private void WhenRegister()
        {
            _result = _accountControllerMock.AccountController.Register(_emailAddress, _password, _repassword).Result;
        }

        //private void ThenResultViewWithEmptyFieldsMessage()
        //{
        //    var resultAsViewResult = Assert.IsType<ViewResult>(_result);

        //    Assert.Equal(3, resultAsViewResult.ViewData.ModelState.ErrorCount);
        //}

        private void ThenResultViewIsSuccessView()
        {
            var resultAsContentResult = Assert.IsType<ContentResult>(_result);
            
            Assert.Equal("Registering user successful", resultAsContentResult.Content);
        }
        
        private void ThenResultViewWithIncorrectFieldMessage(string incorrectMessage)
        {     
            Assert.True(GetMessages().Contains(incorrectMessage), string.Format("There is no message '{0}'", incorrectMessage));
        }

        private IEnumerable<string> GetMessages()
        {
            return _accountControllerMock.MessageCollection.Messages.Select(message => message.Message);
        }

        #endregion

    }

    public enum FieldEnum
    {
        EmailAddress,
        Password,
        Repassword
    }
}