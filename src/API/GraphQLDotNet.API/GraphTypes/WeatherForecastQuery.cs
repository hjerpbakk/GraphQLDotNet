using System.Threading.Tasks;
using GraphQL.Types;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Services.OpenWeather;

namespace GraphQLDotNet.API.GraphTypes
{
    public sealed class WeatherForecastQuery : ObjectGraphType
    {
        public WeatherForecastQuery(IOpenWeatherClient openWeatherClient)
        {
            Field<WeatherForecastType>("forecast", "Get forecast for the next dayz.",
                new QueryArguments(
                        new QueryArgument<NonNullGraphType<LongGraphType>> { Name = "city_id", Description = "Id of the city to fetch weather from." }
                    ),
                context =>
                {
                    return openWeatherClient.GetWeatherFor(context.GetArgument<long>("city_id"));
                });
            Field<WeatherLocationType>("location", "Location from name",
                new QueryArguments(
                        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name", Description = "Name of the location to fetch." }
                    ),
                context =>
                {
                    return openWeatherClient.GetLoactionFor(context.GetArgument<string>("name"));
                });
            Field<WeatherForecastType>("forecastForLocation", "Get forecast for the next dayz",
                new QueryArguments(
                        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name", Description = "Name of the location from witch to fetch weather." }
                    ),
                context =>
                {
                    return GetWeatherFor(openWeatherClient, context.GetArgument<string>("name"));
                });
        }

        private async Task<WeatherForecast> GetWeatherFor(IOpenWeatherClient openWeatherClient, string name)
        {
            var location = await openWeatherClient.GetLoactionFor(name);
            return await openWeatherClient.GetWeatherFor(location.Id);
        }
    }
}
