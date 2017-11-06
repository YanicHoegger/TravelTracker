using System;
namespace TravelTracker.User
{
    public class UserBinderException : Exception
    {
        public UserBinderException(string message) : base(message)
        {
        }
    }
}
