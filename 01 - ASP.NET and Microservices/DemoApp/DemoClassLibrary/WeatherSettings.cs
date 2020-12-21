using System;
using System.Collections.Generic;
using System.Text;

namespace DemoClassLibrary
{
    public class WeatherOptions
    {
        public const string SectionName = "WeatherSettings";

        public string DefaultCity { get; set; }
        public float Factor { get; set; }
    }
}
