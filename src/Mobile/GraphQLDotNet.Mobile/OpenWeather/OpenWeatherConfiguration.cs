using Flurl;
using GraphQLDotNet.Mobile.Helpers;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    internal sealed class OpenWeatherConfiguration
    {
        private const string ApiEndpoint = "graphql";

        public static string GetIconURL(string openWeatherIcon) => $"https://openweathermap.org/img/wn/{openWeatherIcon}@2x.png";

#pragma warning disable RECS0065 // Expression is always 'true' or always 'false'
#pragma warning disable RECS0110 // Condition is always 'true' or always 'false'
        public string GraphQLApiUrl => Url.Combine(
            Secrets.ApiBaseAddress == "localhost"
                ? Device.RuntimePlatform == Device.Android
                    ? "http://10.0.2.2:5000"
                    : "http://localhost:5000"
                : Url.Combine(Secrets.ApiBaseAddress, ApiEndpoint)
            , ApiEndpoint);
#pragma warning restore RECS0110 // Condition is always 'true' or always 'false'
#pragma warning restore RECS0065 // Expression is always 'true' or always 'false'
    }
}
