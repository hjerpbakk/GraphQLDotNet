using System;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.ViewModels.Commands;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Messages;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using Humanizer;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public sealed class WeatherViewModel : PageViewModelBase<WeatherSummaryViewModel>
    {
        private readonly INavigationService navigationService;
        private readonly IOpenWeatherClient openWeatherClient;

        private WeatherSummaryViewModel weatherSummary;
        private WeatherForecast? weatherForecast;

        public WeatherViewModel(INavigationService navigationService, IOpenWeatherClient openWeatherClient)
        {
            this.navigationService = navigationService;
            this.openWeatherClient = openWeatherClient;
            weatherSummary = WeatherSummaryViewModel.Default;
        }

        public override async Task Initialize(WeatherSummaryViewModel argument)
        {
            // TODO: Support map
            weatherSummary = argument;
            Title = argument.Name;
            TriggerPropertyChanged();
            await RefreshCommand.ExecuteAsync();
        }

        public string Temperature => weatherForecast == null
            ? weatherSummary.Temperature
            : weatherForecast.Temperature + "° C";

        public string Icon => weatherForecast == null
            ? weatherSummary.OpenWeatherIcon
            : OpenWeatherConfiguration.GetIconURL(weatherForecast.OpenWeatherIcon);

        public string? Description => weatherForecast?.Description.Humanize();

        public string? TempMin => weatherForecast?.TempMin + "° C";

        public string? TempMax => weatherForecast?.TempMax + "° C";

        public string? Pressure => weatherForecast?.Pressure.ToString("#,##0") + " hPA";

        public string? Humidity => weatherForecast?.Humidity + " %";

        public string? Sunrise => ToTimeAtLocation(weatherForecast?.Sunrise);

        public string? Sunset => ToTimeAtLocation(weatherForecast?.Sunset);

        public string? Wind => GetWind();

        public string? Visibility => weatherForecast?.Visibility.ToString("#,##0") + " m";

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        public IAsyncCommand RefreshCommand => new AsyncCommand(async () =>
            {
                try
                {
                    weatherForecast = await openWeatherClient.GetWeatherForecast(weatherSummary.Id);
                    TriggerPropertyChanged();
                }
                finally
                {
                    IsRefreshing = false;
                }
            });

        public IAsyncCommand RemoveCommand => new AsyncCommand(async () =>
            {
                var locationId = weatherForecast == null
                    ? weatherSummary.Id
                    : weatherForecast.Id;
                MessagingCenter.Send(this,
                    nameof(RemoveLocationMessage),
                    new RemoveLocationMessage(locationId));
                await navigationService.Pop();
            });

        public void TriggerPropertyChanged()
        {
            OnPropertyChanged(
                nameof(Temperature),
                nameof(Icon),
                nameof(Description),
                nameof(TempMin),
                nameof(TempMax),
                nameof(Pressure),
                nameof(Humidity),
                nameof(Sunrise),
                nameof(Sunset),
                nameof(Wind),
                nameof(Visibility));
        }

        private string GetWind()
        {
            const string ms = "m/s";
            if (weatherForecast == null)
            {
                return ms;
            }

            var heading = Convert.ToDouble(weatherForecast?.WindDegrees).ToHeading(HeadingStyle.Abbreviated);
            var windSpeed = Math.Round(weatherForecast!.WindSpeed * 0.514444D, 1);
            return $"{heading} {windSpeed} {ms}";
        }

        private string ToTimeAtLocation(DateTime? utc)
        {
            if (utc == null)
            {
                return " ";
            }

            return (utc.Value + TimeSpan.FromSeconds(weatherForecast!.Timezone)).ToString("HH:mm"); 
        }
    }
}
