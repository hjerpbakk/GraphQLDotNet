namespace GraphQLDotNet.Mobile.ViewModels.Messages
{
    public readonly struct RemoveLocationMessage
    {
        public RemoveLocationMessage(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
