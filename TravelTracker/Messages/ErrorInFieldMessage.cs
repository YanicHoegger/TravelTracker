using System;
namespace TravelTracker.Messages
{
    public class ErrorInFieldMessage : IMessage
    {
        private readonly string _errorMessage;

        public ErrorInFieldMessage(string errorMessage)
        {
            this._errorMessage = errorMessage;
        }

        public string Message 
        {
            get
            {
                return _errorMessage;
            }    
        }
    }
}
