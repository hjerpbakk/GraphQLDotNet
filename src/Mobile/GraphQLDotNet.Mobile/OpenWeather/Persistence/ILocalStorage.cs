using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.Models;

namespace GraphQLDotNet.Mobile.OpenWeather.Persistence
{
    public interface ILocalStorage
    {
        Task Save(IEnumerable<OrderedWeatherSummary> weatherLocationIds);
        OrderedWeatherSummary[] Load();
    }
}
