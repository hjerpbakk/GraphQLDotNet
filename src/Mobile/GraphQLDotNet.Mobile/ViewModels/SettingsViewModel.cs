using System;
using GraphQLDotNet.Mobile.ViewModels.Commands;
using GraphQLDotNet.Mobile.ViewModels.Common;
using Xamarin.Essentials;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class SettingsViewModel : PageViewModelBase
    {
        public IAsyncCommand OpenWebCommand => new AsyncCommand(async () =>
        {
            await Launcher.OpenAsync(new Uri("https://github.com/Sankra/GraphQLDotNet"));
        });
    }
}