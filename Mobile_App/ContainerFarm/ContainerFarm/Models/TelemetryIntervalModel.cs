using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    public static class TelemetryIntervalModel
    {
        public const string TELEMETRY_INTERVAL_PROPERTY = "telemetryInterval";
        public static int TelemetryInterval { get; set; } = 3;

        /// <summary>
        /// True if the telemetry interval value was changed in the settings, otherwise false.
        /// </summary>
        public static bool IsChanged { get; set; }

        /// <summary>
        /// Sets the telemetry interval to the specified value.
        /// </summary>
        /// <param name="newTelemetryInterval">The new telemetry interval value.</param>
        public static void SetTelemetryInterval(int newTelemetryInterval)
        {
            if (newTelemetryInterval < 0) return;

            TelemetryInterval = newTelemetryInterval;
        }
    }
}

