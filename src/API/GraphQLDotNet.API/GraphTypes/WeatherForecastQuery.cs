using GraphQL.Types;
using GraphQLDotNet.Services.OpenWeather;

namespace GraphQLDotNet.API.GraphTypes
{
    public sealed class WeatherForecastQuery : ObjectGraphType
    {
        public WeatherForecastQuery(IOpenWeatherClient openWeatherClient)
        {
            Field<WeatherForecastType>("forecast", "Get the current forecast.",
                new QueryArguments(
                        new QueryArgument<NonNullGraphType<LongGraphType>> { Name = "city_id", Description = "Id of the location from which to fetch forecasts." }
                    ),
                context =>
                {
                    return openWeatherClient.GetWeatherFor(context.GetArgument<long>("city_id"));
                });
            Field<ListGraphType<WeatherSummaryType>>("summaries", "Get forecast summaries for the given locations.",
                new QueryArguments(
                        new QueryArgument<NonNullGraphType<ListGraphType<LongGraphType>>> { Name = "location_ids", Description = "Ids of the locations from which to fetch forecasts." }
                    ),
                context =>
                {
                    return openWeatherClient.GetWeatherSummaryFor(context.GetArgument<long[]>("location_ids"));
                });
            Field<ListGraphType<WeatherLocationType>>("locations", "Get locations with available forecasts.",
                new QueryArguments(
                        new QueryArgument<StringGraphType> { Name = "searchTerms", Description = "Location and optional country after a ,.", DefaultValue = "" },
                        new QueryArgument<IntGraphType> { Name = "maxResults", Description = "The max number of results to return.", DefaultValue = IOpenWeatherClient.MaxNumberOfResults }
                    ),
                context =>
                {
                    return openWeatherClient.GetLocations(context.GetArgument<string>("searchTerms"), context.GetArgument<int>("maxResults"));
                });
        }
    }
}
