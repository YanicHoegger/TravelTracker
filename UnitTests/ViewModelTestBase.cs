using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public abstract class ViewModelTestBase
    {
        protected abstract object ViewModel { get; }

		ValidationContext context;
		List<ValidationResult> result;
		bool isValid;

		protected void WhenValidate()
		{
			context = new ValidationContext(ViewModel, null, null);
			result = new List<ValidationResult>();

			isValid = Validator.TryValidateObject(ViewModel, context, result, true);
		}

		protected void ThenValid()
		{
			Assert.True(isValid);
		}

		protected void ThenInvalid(params string[] errorMessages)
		{
			Assert.False(isValid);

			foreach (var errorMessage in errorMessages)
			{
				Assert.Contains(errorMessage, result.Select(x => x.ErrorMessage));
			}
		}
    }
}
