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
            Field<ListGraphType<WeatherForecastType>>("forecasts", "Get forecasts for the given locations.",
                new QueryArguments(
                        new QueryArgument<NonNullGraphType<ListGraphType<LongGraphType>>> { Name = "location_ids", Description = "Ids of the locations to fetch weather from." }
                    ),
                context =>
                {
                    return openWeatherClient.GetWeatherFor(context.GetArgument<long[]>("location_ids"));
                });
            Field<ListGraphType<WeatherLocationType>>("locations", "Available locations",
                new QueryArguments(
                        new QueryArgument<StringGraphType> { Name = "beginsWith", Description = "Characters the location begins with", DefaultValue = "" },
                        new QueryArgument<IntGraphType> { Name = "maxResults", Description = "Max number of results", DefaultValue = IOpenWeatherClient.MaxNumberOfResults }
                    ),
                context =>
                {
                    return openWeatherClient.GetLocations(context.GetArgument<string>("beginsWith"), context.GetArgument<int>("maxResults"));
                });
        }
    }
}
