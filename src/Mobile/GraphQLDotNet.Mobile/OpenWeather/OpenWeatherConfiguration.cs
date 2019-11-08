using Flurl;
using GraphQLDotNet.Mobile.Helpers;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    internal static class OpenWeatherConfiguration
    {
        private const string ApiEndpoint = "graphql";

        public static string GetIconURL(string openWeatherIcon) => $"https://openweathermap.org/img/wn/{openWeatherIcon}@2x.png";

        // TODO: Make instance method and use this class correctly
#if DEBUG
        public static string GraphQLApiUrl { get; } = Device.RuntimePlatform is Device.Android ? Url.Combine("http://10.0.2.2:5000", ApiEndpoint) : Url.Combine("http://127.0.0.1:5000", ApiEndpoint);
#else
        public static string GraphQLApiUrl => Url.Combine(Secrets.ApiBaseAddress, ApiEndpoint);
#endif
    }
}
