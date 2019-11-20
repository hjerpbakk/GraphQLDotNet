using Flurl;
using GraphQLDotNet.Mobile.Helpers;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    internal sealed class OpenWeatherConfiguration
    {
        private const string ApiEndpoint = "graphql";

        public static string GetIconURL(string openWeatherIcon) => $"https://openweathermap.org/img/wn/{openWeatherIcon}@2x.png";

        public string GraphQLApiUrl => Url.Combine(Secrets.ApiBaseAddress, ApiEndpoint);
    }
}
