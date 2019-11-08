using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.Models;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.OpenWeather.Persistence;
using GraphQLDotNet.Mobile.ViewModels.Commands;
using GraphQLDotNet.Mobile.ViewModels.Messages;
using GraphQLDotNet.Mobile.Views;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class LocationsViewModel : BaseViewModel
    {
        // TODO: Current location øverst
        // TODO: Laste data ved oppstart...
        public LocationsViewModel()
        {
            Title = "Locations";
            AddLocationCommand = new AsyncCommand(ExecuteAddLocationsCommand);
            RefreshCommand = new AsyncCommand(ExecuteRefreshLocations);
            locations = new ObservableCollection<OrderedWeatherSummary>();

            // TODO: Show IsRefreshing animation and do this properly
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            LoadWeatherSummariesFromDisk();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            // TODO: this warning suxx
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            MessagingCenter.Subscribe<AddLocationViewModel, AddLocationMessage>(this, nameof(AddLocationMessage), async (obj, locationMessage) =>
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
            {
                try
                {
                    // TODO: Sjekk om finnes fra før
                    var summary = await OpenWeatherClient.GetWeatherSummaryFor(locationMessage.Id);
                    var orderedSummary = new OrderedWeatherSummary(summary, Locations.Count);                   
                    Locations.Add(orderedSummary);
                    var localStorage = new LocalStorage();
                    await localStorage.Save(Locations);
                }
                catch (Exception)
                {
                    // TODO: Error?!?
                    return;
                }
            });
        }

        public IAsyncCommand AddLocationCommand { get; }
        public IAsyncCommand RefreshCommand { get; }

        ObservableCollection<OrderedWeatherSummary> locations;
        public ObservableCollection<OrderedWeatherSummary> Locations
        {
            get { return locations; }
            set { SetProperty(ref locations, value); }
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        async Task ExecuteAddLocationsCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new AddLocationPage()));
            }
            catch (Exception ex)
            {
                // TODO: Error
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteRefreshLocations()
        {
            try
            {
                // TODO: if service fails, list becomes empty 
                var weatherSummaries = await OpenWeatherClient.GetWeatherUpdatesFor(Locations.Select(w => w.Id));
                var updatedWeather =
                    from orderedWeatherSummary in Locations
                    join summary in weatherSummaries on orderedWeatherSummary.Id equals summary.Id
                    orderby orderedWeatherSummary.Ordering
                    select orderedWeatherSummary.UpdateWeather(summary);
                // TODO: Save updated values here or wait for app shutdown??
                Locations = new ObservableCollection<OrderedWeatherSummary>(updatedWeather);
            }
            catch (Exception ex)
            {
                // TODO: Error
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        async Task LoadWeatherSummariesFromDisk()
        {
            IsRefreshing = true;
            // TODO: Hent local storage på ordentlig vis i starten
            var weatherSummaries = new LocalStorage().Load();
            if (weatherSummaries.Length == 0)
            {
                IsRefreshing = false;
                return;
            }

            Locations = new ObservableCollection<OrderedWeatherSummary>(weatherSummaries.OrderBy(w => w.Ordering));
            await ExecuteRefreshLocations();
        }
    }
}
