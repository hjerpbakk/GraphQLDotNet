namespace GraphQLDotNet.Mobile.Models
{
    public class Location
    {
        public Location(string name, string country)
        {
            Name = name;
            Country = country;
        }

        public string Name { get; }
        public string Country { get; }
    }
}
