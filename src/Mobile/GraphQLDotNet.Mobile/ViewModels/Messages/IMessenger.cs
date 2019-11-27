using System;
using System.Threading.Tasks;

namespace GraphQLDotNet.Mobile.ViewModels.Messages
{
    public interface IMessenger
    {
        void Subscribe<TMessage>(Func<TMessage, Task> callback)
            where TMessage : IMessage;
        void Publish<TMessage>(TMessage message)
            where TMessage : IMessage;
    }
}
