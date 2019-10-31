using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GraphQLDotNet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController()
        {

        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get(DateTime dateTime)
        {
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

            if (dateTime == DateTime.MinValue)
            {
                return forecasts.ToArray();
            }

            return new[] { forecasts.SingleOrDefault(f => f.Date.Date == dateTime.Date) };
        }
    }
}
