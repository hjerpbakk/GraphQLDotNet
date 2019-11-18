using System.Threading.Tasks;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    public interface ICountryLocator
    {
        Task<string> GetCurrentCountry();
    }
}
