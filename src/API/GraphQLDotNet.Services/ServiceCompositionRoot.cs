using GraphQLDotNet.Services.OpenWeather;
using LightInject;

namespace GraphQLDotNet.Services
{
    public sealed class ServiceCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            //serviceRegistry.Decorate(typeof(IOpenWeatherClient), typeof(CachedOpenWeatherClient));
        }
    }
}
