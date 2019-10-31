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
                new QueryArguments(
                        new QueryArgument<DateGraphType> { Name = "date", Description = "The Date of the forecast." }
                    ),
                context =>
                {
                    return new WeatherForecastController().Get(context.GetArgument<DateTime>("date"));
                });
        }
    }
}
