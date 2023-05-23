using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Enums
{
    /// <summary>
    /// The devices threshold warning colors.
    /// </summary>
    public static class ThresholdValueColors
    {
        // The hex warning colors
        private static readonly string HIGH_COLOR_HEX = "#F20000";
        private static readonly string LOW_COLOR_HEX = "#F29D00";

        // The threshold value colors
        public static readonly Color TEMPERATURE_HIGH = Color.FromArgb(HIGH_COLOR_HEX);
        public static readonly Color TEMPERATURE_LOW = Color.FromArgb(LOW_COLOR_HEX);

        public static readonly Color HUMIDITY_HIGH = Color.FromArgb(HIGH_COLOR_HEX);
        public static readonly Color HUMIDITY_LOW = Color.FromArgb(LOW_COLOR_HEX);

        public static readonly Color WATER_LEVEL_HIGH = Color.FromArgb(HIGH_COLOR_HEX);
        public static readonly Color WATER_LEVEL_LOW = Color.FromArgb(LOW_COLOR_HEX);
    }
}
