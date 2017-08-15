using System;
using System.Collections.Generic;

namespace TravelTracker.Messages
{
    public interface IMessageCollection
    {
        IEnumerable<IMessage> Messages { get; }
        void Add(IMessage message);
    }
}
