using System;
using GraphQL.Types;
using GraphQLDotNet.API.GraphTypes;

namespace GraphQLDotNet.API.Schemas
{
    public class WeatherForecastSchema : Schema
    {
        static readonly Lazy<WeatherForecastQuery> queryHolder = new Lazy<WeatherForecastQuery>(new WeatherForecastQuery());

        public WeatherForecastSchema()
        {
            Query = queryHolder.Value;
            
        }
    }
}
