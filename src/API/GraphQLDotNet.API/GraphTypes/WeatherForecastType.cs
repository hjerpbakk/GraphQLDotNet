using GraphQL.Types;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.API.GraphTypes
{
    public class WeatherForecastType : ObjectGraphType<WeatherForecast>
    {
        public WeatherForecastType()
        {
            Field(x => x.Id).Description("The Id of the forecast location.");
            Field(x => x.Date).Description("The Date of the forecast.");
            Field(x => x.Temperature).Description("The Temperature in C.");
            Field(x => x.TemperatureF).Description("The Temperature in F.");
            Field(x => x.Summary).Description("A textual summary.");
            Field(x => x.OpenWeatherIcon).Description("Icon id of the weather.");
            Field(x => x.Location).Description("Location of the measurement.");
        }
    }
}
