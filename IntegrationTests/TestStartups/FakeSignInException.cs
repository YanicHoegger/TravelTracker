using System;
namespace IntegrationTests.TestStartups
{
    public class FakeSignInException : Exception
    { 
        public FakeSignInException(string message) : base(message)
        {
        }
    }
}
