using System;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels.Common;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels.Messages
{
    public sealed class Messenger : IMessenger
    {
        public void Subscribe<TPublisher, TMessage>(object subscriber, Func<TMessage, Task> callback)
            where TPublisher : class
            where TMessage : IMessage
            => MessagingCenter.Subscribe<TPublisher, TMessage>(subscriber,
                typeof(TMessage).Name,
                (_, message) => callback(message).FireAndForgetSafeAsync());

        public void Publish<TPublisher, TMessage>(TPublisher sender, TMessage message)
            where TPublisher : class
            where TMessage : IMessage
            => MessagingCenter.Send(sender, typeof(TMessage).Name, message);
    }
}
