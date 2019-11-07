using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GraphQLDotNet.Mobile.Models;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.Services;
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
            locations = new ObservableCollection<WeatherSummary>();

            // TODO: Show IsRefreshing animation and do this properly
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ExecuteRefreshLocations();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            // TODO: this warning suxx
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            MessagingCenter.Subscribe<AddLocationViewModel, AddLocationMessage>(this, nameof(AddLocationMessage), async (obj, locationMessage) =>
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
            {
                try
                {
                    var summary = await OpenWeatherClient.GetWeatherSummaryFor(locationMessage.Id);
                    // TODO: Sjekk om finnes fra før
                    Locations.Add(summary);
                    // TODO: Save location here
                    var localStorage = new LocalStorage();
                    await localStorage.Save(Locations.Select(l => l.Id));
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

        ObservableCollection<WeatherSummary> locations;
        public ObservableCollection<WeatherSummary> Locations
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
                // TODO: Hent local storage på ordentlig vis
                var weatherLocationIds = await new LocalStorage().Load();
                if (weatherLocationIds.Length == 0)
                {
                    // TODO: Make sure instructions are shown
                    return;
                }

                // TODO: preserve list ordering after refresh and between runs
                var weatherSummaries = await OpenWeatherClient.GetWeatherSummaryFor(weatherLocationIds);
                Locations = new ObservableCollection<WeatherSummary>(weatherSummaries);
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
