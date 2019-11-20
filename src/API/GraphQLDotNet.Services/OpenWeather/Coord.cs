namespace GraphQLDotNet.Services.OpenWeather
{
    internal readonly struct Coord
    {
        public Coord(double lon, double lat)
        {
            Lon = lon;
            Lat = lat;
        }

        public double Lon { get; }
        public double Lat { get; }
    }
}
