using GraphQL.Types;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.API.GraphTypes
{
    public class WeatherLocationType : ObjectGraphType<WeatherLocation>
    {
        public WeatherLocationType()
        {
            Field(x => x.Id).Description("Location Id.");
            Field(x => x.Name).Description("Location Name.");
            Field(x => x.Country).Description("Residing country.");
            Field(x => x.Latitude).Description("Latitude");
            Field(x => x.Longitude).Description("Longitude");
        }
    }
}
