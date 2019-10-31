using System;
using GraphQL.Types;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.API.GraphTypes
{
    public class WeatherForecastType : ObjectGraphType<WeatherForecast>
    {
        public WeatherForecastType()
        {
            Field(x => x.Date).Description("The Date of the forecast.");
            Field(x => x.TemperatureC).Description("The Temperature in C.");
            Field(x => x.TemperatureF).Description("The Temperature in F.");
            Field(x => x.Summary).Description("A textual summary.");
        }
    }
}
