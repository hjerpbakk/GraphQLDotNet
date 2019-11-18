using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.Models;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.OpenWeather.Persistence;
using GraphQLDotNet.Mobile.ViewModels.Commands;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Messages;
using GraphQLDotNet.Mobile.Views;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class LocationsViewModel : ViewModelBase
    {
        private readonly ILocalStorage localStorage;

        // TODO: Current location øverst
        // TODO: Laste data ved oppstart...
        public LocationsViewModel(ILocalStorage localStorage)
        {
            this.localStorage = localStorage;
            Title = "Locations";
            AddLocationCommand = new AsyncCommand(ExecuteAddLocationsCommand);
            RefreshCommand = new AsyncCommand(ExecuteRefreshLocations);
            locations = new ObservableCollection<OrderedWeatherSummary>();

            // TODO: this warning suxx
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            MessagingCenter.Subscribe<AddLocationViewModel, AddLocationMessage>(this, nameof(AddLocationMessage), async (obj, locationMessage) =>
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
            {
                try
                {
                    // TODO: Sjekk om finnes fra før før legges til
                    var summary = await OpenWeatherClient.GetWeatherSummaryFor(locationMessage.Id);
                    var orderedSummary = new OrderedWeatherSummary(summary, Locations.Count);
                    Locations.Add(orderedSummary);
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
        public AsyncCommand<OrderedWeatherSummary> RemoveLocationCommand =>
            new AsyncCommand<OrderedWeatherSummary>(async (OrderedWeatherSummary weatherSummaryToDelete) =>
            {
                if (Locations.Remove(weatherSummaryToDelete))
                {
                    await localStorage.Save(Locations);
                }
            });

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

        public async override Task Initialize()
        {
            var weatherSummaries = localStorage.Load();
            Locations = new ObservableCollection<OrderedWeatherSummary>(weatherSummaries.OrderBy(w => w.Ordering));
            if (weatherSummaries.Length == 0)
            {
                return;
            }

            await ExecuteRefreshLocations();
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
                if (Locations.Count == 0)
                {
                    return;
                }

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
    }
}
