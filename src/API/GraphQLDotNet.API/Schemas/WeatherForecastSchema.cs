using GraphQL.Types;
using GraphQLDotNet.API.GraphTypes;

namespace GraphQLDotNet.API.Schemas
{
    public class WeatherForecastSchema : Schema
    {
        public WeatherForecastSchema(WeatherForecastQuery weatherForecastQuery)
        {
            Query = weatherForecastQuery;
        }
    }
}
