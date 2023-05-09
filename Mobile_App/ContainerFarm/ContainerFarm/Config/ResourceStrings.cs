using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Config
{
    static internal class ResourceStrings
    {
        static public string AuthorizedDomain { get; set; } = "container-farms.firebaseapp.com";
        public static string Firebase_ApiKey { get; set; } = "AIzaSyDHQuUBlz9s5R2Lp7AbzpW3eKJ4MoCt4Ew";

        public static string IoT_Hub_Connection_String { get; set; } = "HostName=IoTHub-Payal-Rathod.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=C6ysrPh2z1h+xKctq5XXIyB7icJAvYpJKfqYPIfwOl8=";

        public static string IoT_Hub_Farm_DeviceId { get; set; } = "Farm";
    }
}
