using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.Models;

namespace GraphQLDotNet.Mobile.Services
{
    public class MockDataStore : IDataStore<WeatherForecast>
    {
        readonly List<WeatherForecast> items;

        public MockDataStore()
        {
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast((WeatherKind)rng.Next(3))
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summaries[rng.Next(summaries.Length)]
            });
            items = forecasts.ToList();
        }

        public async Task<bool> AddItemAsync(WeatherForecast item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(WeatherForecast item)
        {
            var oldItem = items.Where((WeatherForecast arg) => arg.Date.Date == item.Date.Date).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(DateTime date)
        {
            var oldItem = items.Where((WeatherForecast arg) => arg.Date.Date == date.Date).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<WeatherForecast> GetItemAsync(DateTime date)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Date.Date == date.Date));
        }

        public async Task<IEnumerable<WeatherForecast>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}