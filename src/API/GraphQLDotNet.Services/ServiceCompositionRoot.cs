using GraphQLDotNet.Services.OpenWeather;
using LightInject;

namespace GraphQLDotNet.Services
{
    public sealed class ServiceCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IOpenWeatherClient, OpenWeatherClient>(new PerContainerLifetime())
                .Decorate(typeof(IOpenWeatherClient), typeof(CachedOpenWeatherClient));
        }
    }
}
