using GraphQL.Types;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.API.GraphTypes
{
    public class WeatherSummaryType : ObjectGraphType<WeatherSummary>
    {
        public WeatherSummaryType()
        {
            Field(x => x.Id).Description("The id of the forecast location.");
            Field(x => x.Location).Description("The location of the forecast.");
            Field(x => x.Date).Description("The date and time of the forecast.").Type(new DateTimeGraphType());
            Field(x => x.Temperature).Description("The temperature in C.");
            Field(x => x.OpenWeatherIcon).Description("The icon for the forecast.");
            Field(x => x.Timezone).Description("The timezone of the date and times in this forecast.");
            Field(x => x.Clouds).Description("The cloud coverage in percentage.");
        }
    }
}
