using Xunit;

namespace UnitTests
{
    public static class AssertUrlHelper
    {
        /// <returns>Username</returns>
        public static string Users(string routName, object values)
        {
            Assert.Equal("users", routName);
            //TODO: Check values

            return "userName";
        }
    }
}
