using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace UnitTests
{
    public static class ModelStateDirectoryTestExtensions
    {
        //Method should only be used for tests
        internal static string GetSingleErrorMessage(this ModelStateDictionary modelStateDictionary)
        {
			var modelStateEntry = modelStateDictionary.Values.Single();
            var modelError = modelStateEntry.Errors.Single();

            return modelError.ErrorMessage;
        }
    }
}
