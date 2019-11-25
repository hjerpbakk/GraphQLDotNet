using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels;

namespace GraphQLDotNet.Mobile.OpenWeather.Persistence
{
    public interface ILocalStorage
    {
        Task Save(IEnumerable<WeatherSummaryViewModel> weatherLocationIds);
        WeatherSummaryViewModel[] Load();
    }
}
