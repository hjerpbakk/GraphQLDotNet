namespace GraphQLDotNet.Mobile.OpenWeather
{
    public static class WeatherIcons
    {
        public static string GetFullWeatherIconUrl(string weatherIcon) =>
            $"https://openweathermap.org/img/wn/{weatherIcon}@2x.png";
    }
}
