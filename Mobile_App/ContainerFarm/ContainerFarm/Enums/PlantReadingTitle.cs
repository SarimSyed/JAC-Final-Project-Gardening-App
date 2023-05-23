using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Enums
{
    /// <summary>
    /// Holds the sensor and actuator names of the Plant subsystem.
    /// </summary>
    public static class PlantReadingTitle
    {
        public const string WATER_LEVEL = "water-level-sensor";
        public const string SOIL_MOISTURE = "soil-moisture";
        public const string HUMIDITY = "humidity";
        public const string TEMPERATURE = "temperature";
    }
}
