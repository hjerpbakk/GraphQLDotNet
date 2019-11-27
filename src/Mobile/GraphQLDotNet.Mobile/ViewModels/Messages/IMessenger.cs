using System;
using System.Threading.Tasks;

namespace GraphQLDotNet.Mobile.ViewModels.Messages
{
    public interface IMessenger
    {
        void Subscribe<TPublisher, TMessage>(object subscriber, Func<TMessage, Task> callback)
            where TPublisher : class
            where TMessage : IMessage;
        void Publish<TPublisher, TMessage>(TPublisher sender, TMessage message)
            where TPublisher : class
            where TMessage : IMessage;
    }
}
