using System.Text.Json.Serialization;

namespace GraphQLDotNet.Services.OpenWeather
{
    internal sealed class Forecast
    {
        public Coord? Coord { get; set; }
        public Weather[]? Weather { get; set; }
        public string? Base { get; set; }
        public Main Main { get; set; } = new Main();
        public long Visibility { get; set; }
        public Wind Wind { get; set; } = new Wind();
        public Rain? Rain { get; set; }
        public Clouds Clouds { get; set; } = new Clouds();
        public long Dt { get; set; }
        public Sys Sys { get; set; } = new Sys();
        public long Timezone { get; set; }
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public long Cod { get; set; }
    }

    internal sealed class Clouds
    {
        public long All { get; set; }
    } 

    internal sealed class Main
    {
        public double Temp { get; set; }
        public long Pressure { get; set; }
        public long Humidity { get; set; }
        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }
        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }
    }

    internal sealed class Rain
    {
    }

    internal sealed class Sys
    {
        public long Type { get; set; }
        public long Id { get; set; }
        public string? Country { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }

    internal sealed class Weather
    {
        public long Id { get; set; }
        public string Main { get; set; } = "";
        public string Description { get; set; } = "";
        public string Icon { get; set; } = "";
    }

    internal sealed class Wind
    {
        public double Speed { get; set; }
        public long Deg { get; set; }
    }
}
