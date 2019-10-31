using System;
using GraphQL.Types;
using GraphQLDotNet.API.Controllers;

namespace GraphQLDotNet.API.GraphTypes
{
    public class WeatherForecastQuery : ObjectGraphType
    {
        public WeatherForecastQuery()
        {
            Field<ListGraphType<WeatherForecastType>>("forecasts", "Get forecasts for the next dayz.",
                resolve: context =>
                {
                    return new WeatherForecastController().Get();
                });
        }
    }
}
