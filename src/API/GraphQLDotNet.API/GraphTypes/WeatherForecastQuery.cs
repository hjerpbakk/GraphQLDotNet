using GraphQL.Types;
using GraphQLDotNet.Services.OpenWeather;

namespace GraphQLDotNet.API.GraphTypes
{
    public sealed class WeatherForecastQuery : ObjectGraphType
    {
        public WeatherForecastQuery(IOpenWeatherClient openWeatherClient)
        {
            // TODO: Improve descriptions
            Field<WeatherForecastType>("forecast", "Get forecast for the next dayz.",
                new QueryArguments(
                        new QueryArgument<NonNullGraphType<LongGraphType>> { Name = "city_id", Description = "Id of the city to fetch weather from." }
                    ),
                context =>
                {
                    return openWeatherClient.GetWeatherFor(context.GetArgument<long>("city_id"));
                });
            Field<ListGraphType<WeatherSummaryType>>("summaries", "Get summaries for the given locations.",
                new QueryArguments(
                        new QueryArgument<NonNullGraphType<ListGraphType<LongGraphType>>> { Name = "location_ids", Description = "Ids of the locations to fetch weather from." }
                    ),
                context =>
                {
                    return openWeatherClient.GetWeatherSummaryFor(context.GetArgument<long[]>("location_ids"));
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
