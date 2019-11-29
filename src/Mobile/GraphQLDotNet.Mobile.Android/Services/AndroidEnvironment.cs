using GraphQLDotNet.Mobile.Droid.Services;
using GraphQLDotNet.Mobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidEnvironment))]
namespace GraphQLDotNet.Mobile.Droid.Services
{
    public class AndroidEnvironment : IEnvironment
    {
        public Theme GetTheme() => Theme.Dark;
    }
}
