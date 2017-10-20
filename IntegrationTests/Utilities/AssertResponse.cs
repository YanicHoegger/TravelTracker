using System.Net.Http;
using Xunit;

namespace IntegrationTests.Utilities
{
    public static class AssertResponse
    {
        public static void Redirect(HttpResponseMessage responseMessage, string expectedUrl)
        {
			Assert.Equal(System.Net.HttpStatusCode.Found, responseMessage.StatusCode);

			Assert.Single(responseMessage.Headers.GetValues("Location"), expectedUrl);
        }
    }
}
