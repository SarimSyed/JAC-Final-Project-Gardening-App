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

        public static void SetTelemetryInterval(int newTelemetryInterval)
        {
            if (newTelemetryInterval < 0) return;

            TelemetryInterval = newTelemetryInterval;
        }
    }
}

