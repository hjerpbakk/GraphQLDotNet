using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    public class AddLocationViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly ICountryLocator countryLocator;

        public AddLocationViewModel(INavigationService navigationService, ICountryLocator countryLocator)
        {
            this.navigationService = navigationService;
            this.countryLocator = countryLocator;
            Title = "Add new location";
            searchResults = new ObservableCollection<WeatherLocation>();
        }

        public override async Task Initialize()
        {
            // TODO: Dette funket ikke på første run på verken Android eller iOS
            var currentCountry = await countryLocator.GetCurrentCountry();
            var locations = await OpenWeatherClient.GetLocations($", {currentCountry}");
            SearchResults = new ObservableCollection<WeatherLocation>(locations);
        }

        public IAsyncCommand CancelCommand => new AsyncCommand(
            async () => await navigationService.PopModal());

        // TODO: Use Async-command...
        public ICommand PerformSearch => new Command<TextChangedEventArgs>((TextChangedEventArgs query) =>
        {
            var nameAndCountry = query.NewTextValue.Split(',');
            string currentCountry = nameAndCountry.Length > 1
                ? nameAndCountry[1]
                : countryLocator.GetCurrentCountry().GetAwaiter().GetResult();

            // TODO: Throttle events...
            var results = OpenWeatherClient.GetLocations($"{query.NewTextValue}, {currentCountry}").GetAwaiter().GetResult();

            SearchResults = new ObservableCollection<WeatherLocation>(results);
        });

        public ICommand LocationSelectedCommand => new AsyncCommand<int>(async (int row) =>
        {
            if (row < 0)
            {
                // TODO: logg
                return;
            }

            // TODO: Finn bedre måte å sende dataene på...
            MessagingCenter.Send(this,
                nameof(AddLocationMessage),
                new AddLocationMessage(SearchResults[row].Id));
            await navigationService.PopModal();
        });

        public ICommand OpenLocationInMapsCommand => new AsyncCommand<int>(async (int row) =>
        {
            if (row < 0)
            {
                // TODO: logg
                return;
            }

            var location = SearchResults[row];
            var coordinates = new Location(location.Latitude, location.Longitude);
            var options = new MapLaunchOptions { Name = location.Name };
            await Map.OpenAsync(coordinates, options);
        });

        private ObservableCollection<WeatherLocation> searchResults;
        public ObservableCollection<WeatherLocation> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                searchResults = value;
                OnPropertyChanged();
            }
        }
    }
}
