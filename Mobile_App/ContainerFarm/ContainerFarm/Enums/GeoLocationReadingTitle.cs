using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Enums
{
    /// <summary>
    /// Holds the sensor and actuator names of the GeoLocation subsystem.
    /// </summary>
    public static class GeoLocationReadingTitle
    {
        public const string BUZZER = "buzzer";

        public const string GPS_ADDRESS = "Address";
        public const string GPS_LOCATION = "gps-location";
        public const string PITCH = "pitch";
        public const string ROLL_ANGLE = "roll-angle";
        public const string VIBRATION = "vibration";
    }
}
