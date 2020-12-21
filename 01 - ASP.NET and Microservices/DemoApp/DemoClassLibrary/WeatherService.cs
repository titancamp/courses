using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DemoClassLibrary
{
    public interface IWeatherService
    {
        (string city, float temperature) GetAirTemperature(string city);
    }

    internal class WeatherService : IWeatherService
    {
        private readonly IGeoLocationService locationService;

        private WeatherOptions Settings { get; }

        public WeatherService(IGeoLocationService location, IOptions<WeatherOptions> options)
        {
            locationService = location;

            Settings = options.Value;
        }

        public (string city, float temperature) GetAirTemperature(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                city = Settings.DefaultCity;
            }

            GeoLocation cityLocation = locationService.GetLocation(city);

            return (city, GetLocationTemperature(cityLocation));
        }

        private float GetLocationTemperature(GeoLocation location)
        {
            return (location.Latitude + location.Longitude) / Settings.Factor;
        }
    }
}
