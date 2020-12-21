using System;
using System.Collections.Generic;
using System.Text;

namespace DemoClassLibrary
{
    public struct GeoLocation
    {
        public GeoLocation(float latitude, float longitude)
        {

            Latitude = latitude;
            Longitude = longitude;
        }
        public float Latitude { get; }

        public float Longitude { get; }
    }

    public class GeoLocationService : IGeoLocationService
    {
        public GeoLocation GetLocation(string city)
        {
            return city switch
            {
                "Yerevan" => new GeoLocation(40.18f, 44.5f),
                "Gyumri" => new GeoLocation(40.8f, 43.8f),
                _ => throw new NotSupportedException($"Location of city {city} is not supported"),
            };
        }
    }

    public interface IGeoLocationService
    {
        GeoLocation GetLocation(string city);
    }
}
