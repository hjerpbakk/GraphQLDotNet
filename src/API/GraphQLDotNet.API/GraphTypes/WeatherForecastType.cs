using GraphQL.Types;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.API.GraphTypes
{
    public class WeatherForecastType : ObjectGraphType<WeatherForecast>
    {
        public WeatherForecastType()
        {
            Field(x => x.Id).Description("The id of the forecast location.");
            Field(x => x.Date).Description("The date and time of the forecast.").Type(new DateTimeGraphType());
            Field(x => x.Temperature).Description("The temperature in C.");
            Field(x => x.Summary).Description("A textual summary of the forecast.");
            Field(x => x.OpenWeatherIcon).Description("The icon for the forecast.");
            Field(x => x.Location).Description("The location of the forecast.");
            Field(x => x.Description).Description("A textual description of the forecast.");
            Field(x => x.TempMin).Description("The minimum temperature in this nychthemeron.");
            Field(x => x.TempMax).Description("The maximum temperature in this nychthemeron.");
            Field(x => x.Pressure).Description("The current pressure.");
            Field(x => x.Humidity).Description("The current humidity");
            Field(x => x.Sunrise).Description("The time of the sunrise in this nychthemeron.").Type(new DateTimeGraphType());
            Field(x => x.Sunset).Description("The time of the sunset in this nychthemeron.").Type(new DateTimeGraphType());
            Field(x => x.WindSpeed).Description("The current wind speed.");
            Field(x => x.WindDegrees).Description("The current wind direction.");
            Field(x => x.Visibility).Description("The current visibility.");
            Field(x => x.Timezone).Description("The timezone of the date and times in this forecast.");
            Field(x => x.Clouds).Description("The cloud coverage in percentage.");
        }
    }
}
