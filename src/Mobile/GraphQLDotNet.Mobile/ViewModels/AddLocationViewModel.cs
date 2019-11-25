using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
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
    public class AddLocationViewModel : PageViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly ICountryLocator countryLocator;
        private readonly IOpenWeatherClient openWeatherClient;
        private readonly Timer searchTypingTimer;

        private ObservableCollection<WeatherLocation> searchResults;
        private string latestQueryText;

        // TODO: Possible to auto show keyboard?
        public AddLocationViewModel(INavigationService navigationService, ICountryLocator countryLocator, IOpenWeatherClient openWeatherClient)
        {
            this.navigationService = navigationService;
            this.countryLocator = countryLocator;
            this.openWeatherClient = openWeatherClient;
            // TODO: What is a good interval value?
            searchTypingTimer = new Timer { AutoReset = false, Interval = 1D };
            searchTypingTimer.Elapsed += SearchTypingTimer_Elapsed;
            Title = "Add new location";
            searchResults = new ObservableCollection<WeatherLocation>();
            latestQueryText = "";
        }

        public ObservableCollection<WeatherLocation> SearchResults
        {
            get { return searchResults; }
            set { SetProperty(ref searchResults, value); }
        }

        public IAsyncCommand CancelCommand => new AsyncCommand(
            async () => await navigationService.PopModal());

        public ICommand PerformSearch => new Command<TextChangedEventArgs>(
            (TextChangedEventArgs query) =>
            {
                searchTypingTimer.Stop();
                searchTypingTimer.Start();
                latestQueryText = query.NewTextValue;
            });

        public IAsyncCommand<int> LocationSelectedCommand => new AsyncCommand<int>(
            async (int row) =>
            {
                if (row < 0)
                {
                    return;
                }

                // TODO: Finnes det en bedre måte å sende dataene på? Gjøre denne non-static om meldinger er gudd.
                MessagingCenter.Send(this,
                    nameof(AddLocationMessage),
                    new AddLocationMessage(SearchResults[row].Id, SearchResults[row].Name));
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

        private void SearchTypingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            searchTypingTimer.Stop();
            Search().FireAndForgetSafeAsync();
        }

        private async Task Search()
        {
            // TODO: Cancel previous calls if underway?
            var nameAndCountry = latestQueryText.Split(',');
            string currentCountry = nameAndCountry.Length > 1
                ? nameAndCountry[1]
                : await countryLocator.GetCurrentCountry();
            var searchString = $"{nameAndCountry[0]}, {currentCountry}";
            var results = await openWeatherClient.GetLocations(searchString);
            SearchResults = new ObservableCollection<WeatherLocation>(results);
        }
    }
}
