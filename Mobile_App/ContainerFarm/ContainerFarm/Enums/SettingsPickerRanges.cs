using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Enums
{
    /// <summary>
    /// The settings picker start and count values.
    /// </summary>
    public static class SettingsPickerRanges
    {
        public const int TEMPERATURE_HIGH_START = 15;
        public const int TEMPERATURE_HIGH_COUNT = 36;

        public const int TEMPERATURE_LOW_START = -20;
        public const int TEMPERATURE_LOW_COUNT = 36;

        public const int HUMIDITY_HIGH_START = 50;
        public const int HUMIDITY_HIGH_COUNT = 51;

        public const int HUMIDITY_LOW_START = 0;
        public const int HUMIDITY_LOW_COUNT = 51;

        public const int WATER_LEVEL_HIGH_START = 50;
        public const int WATER_LEVEL_HIGH_COUNT = 51;

        public const int WATER_LEVEL_LOW_START = 0;
        public const int WATER_LEVEL_LOW_COUNT = 51;

        public const int TELEMETRY_INTERVAL_START = 1;
        public const int TELEMETRY_INTERVAL_COUNT = 20;
    }
}
