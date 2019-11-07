namespace GraphQLDotNet.Services.OpenWeather
{
    internal sealed class Location
    {
        // TODO: only get and use ctor?
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Country { get; set; } = "";
        public Coord Coord { get; set; } = new Coord(); 
    }
}
