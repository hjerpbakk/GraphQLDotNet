using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.ViewModels.Commands;
using GraphQLDotNet.Mobile.ViewModels.Messages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class AddLocationViewModel : BaseViewModel
    {
        private readonly CountryLocator countryLocator;

        // TODO: Cancel as command
        public AddLocationViewModel()
        {
            // TODO: use DI
            countryLocator = new CountryLocator();
            Title = "Add new location";
            var currentCountry = countryLocator.GetCurrentCountry().GetAwaiter().GetResult();
            searchResults = new ObservableCollection<WeatherLocation>(OpenWeatherClient.GetLocations($", {currentCountry}").GetAwaiter().GetResult());
            // TODO: Show location on map by clicking (i)-button in list
        }

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

        public ICommand LocationSelectedCommand => new AsyncCommand<SelectedItemChangedEventArgs>(async (SelectedItemChangedEventArgs selectedValue) =>
        {
            // TODO: Finn bedre måte å sende dataene på...
            MessagingCenter.Send(this,
                nameof(AddLocationMessage),
                new AddLocationMessage(((WeatherLocation)selectedValue.SelectedItem).Id));
            await Application.Current.MainPage.Navigation.PopModalAsync();
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
