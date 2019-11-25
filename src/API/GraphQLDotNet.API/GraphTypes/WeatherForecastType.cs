using GraphQL.Types;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.API.GraphTypes
{
    public class WeatherForecastType : ObjectGraphType<WeatherForecast>
    {
        public WeatherForecastType()
        {
            // TODO: Better field descriptions
            Field(x => x.Id).Description("The Id of the forecast location.");
            Field(x => x.Date).Description("The Date of the forecast.").Type(new DateTimeGraphType());
            Field(x => x.Temperature).Description("The Temperature in C.");
            Field(x => x.TemperatureF).Description("The Temperature in F.");
            Field(x => x.Summary).Description("A textual summary.");
            Field(x => x.OpenWeatherIcon).Description("Icon id of the weather.");
            Field(x => x.Location).Description("Location of the measurement.");
            Field(x => x.Description).Description("Description of the measurement.");
            Field(x => x.TempMin).Description("The TempMin");
            Field(x => x.TempMax).Description("The TempMax");
            Field(x => x.Pressure).Description("The Pressure");
            Field(x => x.Humidity).Description("The Humidity");
            Field(x => x.Sunrise).Description("The Sunrise").Type(new DateTimeGraphType());
            Field(x => x.Sunset).Description("The Sunset").Type(new DateTimeGraphType());
            Field(x => x.WindSpeed).Description("The WindSpeed");
            Field(x => x.WindDegrees).Description("The WindDegrees");
            Field(x => x.Visibility).Description("The Visibility");
            Field(x => x.Timezone).Description("The Timezone");
            Field(x => x.Clouds).Description("The cloud coverage in percentage");
        }
    }
}
