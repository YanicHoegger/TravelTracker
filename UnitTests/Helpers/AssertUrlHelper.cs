using System.Reflection;
using Xunit;

namespace UnitTests.Helpers
{
    public static class AssertUrlHelper
    {
        /// <returns>Username</returns>
        public static string Users(string routName, object values)
        {
            Assert.Equal("users", routName);

            var valuesType = values.GetType();

            var actionPropertyInfo = valuesType.GetProperty("action");
            Assert.Equal("", actionPropertyInfo.GetValue(values));

            var userNamePropertyInfo = valuesType.GetProperty("username");
            return (string)userNamePropertyInfo.GetValue(values);
        }
    }
}
