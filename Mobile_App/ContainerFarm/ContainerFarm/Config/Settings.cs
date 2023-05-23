using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Config
{
    public class Settings
    {
         //Event Hub Connection Strings 
        public string EventHubConnectionString { get; set; }
        public string EventHubName { get; set; }
        public string ConsumerGroup { get; set; }
        public string StorageConnectionString { get; set; }
        public string BlobContainerName { get; set; }

        //IoT Hub Connection Strings
        public string HubConnectionString { get; set; }
        public string DeviceId { get; set; }

        // Thresholds
        public double TemperatureHighThreshold { get; set; } = 40;
        public double TemperatureLowThreshold { get; set; } = 15;
        public double HumidityHighThreshold { get; set; } = 60;
        public double HumidityLowThreshold { get; set; } = 30;
        public double WaterLevelHighThreshold { get; set; } = 50;
        public double WaterLevelLowThreshold { get; set; } = 20;
    }
}
