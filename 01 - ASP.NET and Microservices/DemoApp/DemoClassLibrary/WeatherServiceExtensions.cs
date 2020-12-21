using DemoClassLibrary;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Extensions
{
    public static class WeatherServiceExtensions
    {
        public static IServiceCollection AddWeatherServices(this IServiceCollection services)
        {
            services.AddSingleton<IGeoLocationService, GeoLocationService>();
            services.AddSingleton<IWeatherService, WeatherService>();
            return services;
        }
    }
}
