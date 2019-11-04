namespace GraphQLDotNet.Contracts
{
    public sealed class WeatherLocation
    {
        public WeatherLocation(long id, string name, string country)
        {
            Id = id;
            Name = name;
            Country = country;
        }

        public long Id { get; }
        public string Name { get; }
        public string Country { get; }
    }
}
