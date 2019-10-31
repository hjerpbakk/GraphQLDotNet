using System;
using System.Collections.Generic;
using System.Linq;
using GraphQLDotNet.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLDotNet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<WeatherForecast> Get(DateTime dateTime)
        {
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast((WeatherKind)rng.Next(3))
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summaries[rng.Next(summaries.Length)]
            });

            if (dateTime == DateTime.MinValue)
            {
                return forecasts.ToArray();
            }

            return new[] { forecasts.SingleOrDefault(f => f.Date.Date == dateTime.Date) };
        }
    }
}
