namespace GraphQLDotNet.Mobile.ViewModels.Messages
{
    public readonly struct AddLocationMessage : IMessage
    {
        public AddLocationMessage(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; }
        public string Name { get; }
    }
}
