namespace GraphQLDotNet.Mobile.ViewModels.Messages
{
    public readonly struct AddLocationMessage
    {
        public AddLocationMessage(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
