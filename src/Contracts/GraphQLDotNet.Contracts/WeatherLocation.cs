namespace GraphQLDotNet.Contracts
{
    public sealed class WeatherLocation
    {
        public WeatherLocation(long id, string name, string country, double latitude, double longitude)
        {
            Id = id;
            Name = name;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
        }

        public long Id { get; }
        public string Name { get; }
        public string Country { get; }
        public double Latitude { get; }
        public double Longitude { get; }
    }
}
