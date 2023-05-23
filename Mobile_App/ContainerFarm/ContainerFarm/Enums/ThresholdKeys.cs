using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Enums
{
    /// <summary>
    /// The keys for the threshold preferences.
    /// </summary>
    public static class ThresholdKeys
    {
        public const string TEMPERATURE_HIGH = "temperatureHigh";
        public const string TEMPERATURE_LOW = "temperatureLow";
        public const string HUMIDITY_HIGH = "humidityHigh";
        public const string HUMIDITY_LOW = "humidityLow";
        public const string WATER_LEVEL_HIGH = "waterLevelHigh";
        public const string WATER_LEVEL_LOW = "waterLevelLow";
    }
}
