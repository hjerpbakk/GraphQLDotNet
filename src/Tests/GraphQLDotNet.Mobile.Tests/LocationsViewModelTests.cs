using FluentAssertions;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.OpenWeather.Persistence;
using GraphQLDotNet.Mobile.ViewModels;
using GraphQLDotNet.Mobile.ViewModels.Messages;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using Moq;
using NotifyPropertyChanged.Verifier;
using Xunit;

namespace GraphQLDotNet.Mobile.Tests
{
    public sealed class LocationsViewModelTests
    {
        private readonly LocationsViewModel viewModel;

        readonly Messenger messenger;

        public LocationsViewModelTests()
        {
            var navigationServiceFake = new Mock<INavigationService>();
            var localStorageFake = new Mock<ILocalStorage>();
            var openWeatherClientFake = new Mock<IOpenWeatherClient>();
            messenger = new Messenger();
            viewModel = new LocationsViewModel(navigationServiceFake.Object,
                localStorageFake.Object,
                openWeatherClientFake.Object,
                messenger);
        }

        [Fact]
        public void IsRefreshing_Notifies()
        {
            viewModel.ShouldNotifyOn(vm => vm.IsRefreshing)
                .When(vm => vm.IsRefreshing = true);
        }

        [Fact]
        public void AddANewLocation()
        {
            viewModel.Locations.Should().BeEmpty();

            messenger.Publish(new AddLocationMessage(1, "test"));

            viewModel.Locations.Should().ContainSingle();
        }
    }
}
