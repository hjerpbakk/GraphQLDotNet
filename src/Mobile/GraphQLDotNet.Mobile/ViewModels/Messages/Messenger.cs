using System;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels.Common;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels.Messages
{
    public sealed class Messenger : IMessenger
    {
        public void Subscribe<TMessage>(Func<TMessage, Task> callback)
            where TMessage : IMessage
            => MessagingCenter.Subscribe<Messenger, TMessage>(this,
                typeof(TMessage).Name,
                (_, message) => callback(message).FireAndForgetSafeAsync());

        public void Publish<TMessage>(TMessage message)
            where TMessage : IMessage
            => MessagingCenter.Send(this, typeof(TMessage).Name, message);
    }
}
