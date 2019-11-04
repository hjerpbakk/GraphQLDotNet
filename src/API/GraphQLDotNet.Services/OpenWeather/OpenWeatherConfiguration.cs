using System;

namespace GraphQLDotNet.Services.OpenWeather
{
    public sealed class OpenWeatherConfiguration
    {
        public string ApiKey { get; set; } = "";

        public string BaseUrl => "https://api.openweathermap.org";

        public string OpenWeatherURL => "/data/2.5/weather?id={0}&units=metric&APPID=" + ApiKey;

        public string CitiesFile = "city.list.json";

        public void Verify()
        {
            foreach (var property in GetType().GetProperties())
            {
                if (string.IsNullOrEmpty((string)property.GetValue(this)))
                {
                    throw new Exception($"{property.Name} is missing or empty.");
                }
            }
        }
    }
}
