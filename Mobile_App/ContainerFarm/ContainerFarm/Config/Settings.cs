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
        public int TemperatureHighThreshold { get; set; } = 40;
        public int TemperatureLowThreshold { get; set; } = 15;
        public int HumidityHighThreshold { get; set; } = 60;
        public int HumidityLowThreshold { get; set; } = 30;
        public int WaterLevelHighThreshold { get; set; } = 50;
        public int WaterLevelLowThreshold { get; set; } = 20;
    }
}
