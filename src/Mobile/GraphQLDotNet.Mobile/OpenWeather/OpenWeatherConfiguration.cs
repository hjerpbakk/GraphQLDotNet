namespace GraphQLDotNet.Mobile.OpenWeather
{
    internal static class OpenWeatherConfiguration
    {
        public static string GetIconURL(string openWeatherIcon) => $"https://openweathermap.org/img/wn/{openWeatherIcon}@2x.png";
    }
}
