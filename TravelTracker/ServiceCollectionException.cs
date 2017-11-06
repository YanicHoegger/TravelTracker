using System;
namespace TravelTracker
{
    public class ServiceCollectionException : Exception
    {
        public ServiceCollectionException(string message) : base(message) 
        {
        }
    }
}
