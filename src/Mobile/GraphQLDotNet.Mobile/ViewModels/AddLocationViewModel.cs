using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.ViewModels.Commands;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Messages;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class AddLocationViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly ICountryLocator countryLocator;
        private readonly IOpenWeatherClient openWeatherClient;
        private ObservableCollection<WeatherLocation> searchResults;

        public AddLocationViewModel(INavigationService navigationService, ICountryLocator countryLocator, IOpenWeatherClient openWeatherClient)
        {
            this.navigationService = navigationService;
            this.countryLocator = countryLocator;
            this.openWeatherClient = openWeatherClient;
            Title = "Add new location";
            searchResults = new ObservableCollection<WeatherLocation>();
        }

        public ObservableCollection<WeatherLocation> SearchResults
        {
            get { return searchResults; }
            set { SetProperty(ref searchResults, value); }
        }

        public IAsyncCommand CancelCommand => new AsyncCommand(
            async () => await navigationService.PopModal());

        public IAsyncCommand<TextChangedEventArgs> PerformSearch => new AsyncCommand<TextChangedEventArgs>(
            async (TextChangedEventArgs query) =>
            {
                var nameAndCountry = query.NewTextValue.Split(',');
                string currentCountry = nameAndCountry.Length > 1
                    ? nameAndCountry[1]
                    : countryLocator.GetCurrentCountry().GetAwaiter().GetResult();

                // TODO: Throttle events...
                var results = await openWeatherClient.GetLocations($"{query.NewTextValue}, {currentCountry}");

                SearchResults = new ObservableCollection<WeatherLocation>(results);
            });

        public IAsyncCommand<int> LocationSelectedCommand => new AsyncCommand<int>(
            async (int row) =>
            {
                if (row < 0)
                {
                    return;
                }

                // TODO: Finnes det en bedre måte å sende dataene på?
                MessagingCenter.Send(this,
                    nameof(AddLocationMessage),
                    new AddLocationMessage(SearchResults[row].Id));
                await navigationService.PopModal();
            });

        public IAsyncCommand<int> OpenLocationInMapsCommand => new AsyncCommand<int>(
            async (int row) =>
            {
                if (row < 0)
                {
                    return;
                }

                var location = SearchResults[row];
                var coordinates = new Location(location.Latitude, location.Longitude);
                var options = new MapLaunchOptions { Name = location.Name };
                await Map.OpenAsync(coordinates, options);
            });

        public override async Task Initialize()
        {
            // TODO: Dette funket ikke på første run på verken Android eller iOS
            var currentCountry = await countryLocator.GetCurrentCountry();
            var locations = await openWeatherClient.GetLocations($", {currentCountry}");
            SearchResults = new ObservableCollection<WeatherLocation>(locations);
        }
    }
}
