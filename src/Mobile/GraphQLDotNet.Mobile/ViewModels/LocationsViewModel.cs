using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.Models;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.OpenWeather.Persistence;
using GraphQLDotNet.Mobile.ViewModels.Commands;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Messages;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class LocationsViewModel : PageViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly ILocalStorage localStorage;
        private readonly IOpenWeatherClient openWeatherClient;
        private ObservableCollection<WeatherSummaryViewModel> locations;
        private bool isRefreshing;

        // TODO: Current location øverst
        // TODO: Laste data ved oppstart...
        public LocationsViewModel(INavigationService navigationService, ILocalStorage localStorage, IOpenWeatherClient openWeatherClient)
        {
            this.navigationService = navigationService;
            this.localStorage = localStorage;
            this.openWeatherClient = openWeatherClient;
            Title = "Locations";
            RefreshCommand = new AsyncCommand(ExecuteRefreshLocations);
            locations = new ObservableCollection<WeatherSummaryViewModel>();

            // TODO: this warning suxx
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            MessagingCenter.Subscribe<AddLocationViewModel, AddLocationMessage>(this, nameof(AddLocationMessage), async (obj, locationMessage) =>
            {
                if (Locations.Contains(new WeatherSummaryViewModel(locationMessage.Id)))
                {
                    return;
                }

                var orderedSummary = new WeatherSummaryViewModel(locationMessage.Id, locationMessage.Name, Locations.Count);
                Locations.Add(orderedSummary);
                var summary = await openWeatherClient.GetWeatherSummaryFor(locationMessage.Id);
                if (summary != WeatherSummary.Default)
                {
                    orderedSummary.Temperature = summary.Temperature;
                    orderedSummary.OpenWeatherIcon = summary.OpenWeatherIcon;
                    OnPropertyChanged("Temperature");
                }

                await localStorage.Save(Locations);
            });

            MessagingCenter.Subscribe<WeatherViewModel, RemoveLocationMessage>(this, nameof(RemoveLocationMessage), async (obj, locationMessage) =>
            {
                await RemoveLocationCommand.ExecuteAsync(new WeatherSummaryViewModel(locationMessage.Id));
            });
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        }

        public IAsyncCommand AddLocationCommand => new AsyncCommand(
            async () => await navigationService.NavigateModallyTo<AddLocationViewModel>());

        public AsyncCommand<WeatherSummaryViewModel> RemoveLocationCommand => new AsyncCommand<WeatherSummaryViewModel>(
            async (WeatherSummaryViewModel weatherSummaryToDelete) =>
            {
                if (Locations.Remove(weatherSummaryToDelete))
                {
                    await localStorage.Save(Locations);
                }
            });

        public IAsyncCommand SelectionChangedCommand => new AsyncCommand(
            async () =>
            {
                if (SelectedLocation as object == null)
                {
                    return;
                }

                await navigationService.NavigateTo<WeatherViewModel, WeatherSummaryViewModel>(SelectedLocation!);
                SelectedLocation = null;
                OnPropertyChanged(nameof(SelectedLocation));
            });

        public IAsyncCommand RefreshCommand { get; }

        public WeatherSummaryViewModel? SelectedLocation { get; set; }

        public ObservableCollection<WeatherSummaryViewModel> Locations
        {
            get { return locations; }
            set { SetProperty(ref locations, value); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        public async override Task Initialize()
        {
            var weatherSummaries = localStorage.Load();
            Locations = new ObservableCollection<WeatherSummaryViewModel>(weatherSummaries.OrderBy(w => w.Ordering));
            if (weatherSummaries.Length == 0)
            {
                return;
            }

            await ExecuteRefreshLocations();
        }

        private async Task ExecuteRefreshLocations()
        {
            try
            {
                if (Locations.Count == 0)
                {
                    return;
                }

                var weatherSummaries = await openWeatherClient.GetWeatherUpdatesFor(Locations.Select(w => w.Id));
                if (!weatherSummaries.Any())
                {
                    return;
                }

                var updatedWeather =
                    from orderedWeatherSummary in Locations
                    join summary in weatherSummaries on orderedWeatherSummary.Id equals summary.Id
                    orderby orderedWeatherSummary.Ordering
                    select orderedWeatherSummary.UpdateWeather(summary);
                Locations = new ObservableCollection<WeatherSummaryViewModel>(updatedWeather);
                await localStorage.Save(Locations);
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
