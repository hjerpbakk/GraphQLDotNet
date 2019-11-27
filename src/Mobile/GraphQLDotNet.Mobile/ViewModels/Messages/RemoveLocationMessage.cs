namespace GraphQLDotNet.Mobile.ViewModels.Messages
{
    public readonly struct RemoveLocationMessage : IMessage
    {
        public RemoveLocationMessage(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
