using System;
using System.Collections.Generic;

namespace TravelTracker.Messages
{
    public class MessageCollection : IMessageCollection
    {
        private readonly List<IMessage> _messages = new List<IMessage>();

        public IEnumerable<IMessage> Messages
        {
            get
            {
                return _messages;
            }
        }

        public void Add(IMessage message)
        {
            _messages.Add(message);
        }
    }
}
