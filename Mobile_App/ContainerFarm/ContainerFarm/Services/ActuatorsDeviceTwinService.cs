using ContainerFarm.Models;
using ContainerFarm.Models.Actuators;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Services
{
    public static class ActuatorsDeviceTwinService
    {
        public static RegistryManager RegistryManager;

        /// <summary>
        /// Read and Updates the device twin properties.
        /// </summary>
        /// <param name="twin">The device twin.</param>
        /// <returns></returns>
        public static async Task DeviceTwinLoop(Twin twin)
        {
            // Return if the specified twin is null
            if (twin == null) return;

            TwinCollection desiredProperties = twin.Properties.Desired;

            Console.WriteLine(desiredProperties);

            // Get the property values to updated the desired properties
            string geolocationBuzzer = GeoLocationBuzzerTwin(desiredProperties);
            string securityDoorLock = SecurityDoorLockTwin(twin, desiredProperties);
            string plantsLED = PlantsLEDTwin(twin, desiredProperties);
            string plantsFAN = PlantsFANTwin(twin, desiredProperties);

            // Create the new twin patch
            var patch =
                $@"{{
                    properties: {{
                        desired: {{
                            geolocationBuzzer: ""{geolocationBuzzer}"",
                            securityDoorLock: ""{securityDoorLock}"",
                            plantsLED: ""{plantsLED}"",
                            plantsFan: ""{plantsFAN}"",
                        }}
                    }}
                }}";

            // Update the device twin with the new patch
            await RegistryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);

            Console.WriteLine(twin.Properties.Reported);
        }

        /// <summary>
        /// Updates the GeoLocation buzzer actuator from the specified twin desired properties.
        /// </summary>
        /// <param name="desiredProperties">The desired properties of the device twin.</param>
        /// <returns>The GeoLocation buzzer actuator twin property value.</returns>
        private static string GeoLocationBuzzerTwin(TwinCollection desiredProperties)
        {
            // Get the Geo Location buzzer actuator from the container
            BuzzerActuator geoLocationBuzzer = App.Repo.Containers[0].Location.BuzzerActuator;

            // Don't check twin properties if the actuator was just set to 'on' in the app
            if (geoLocationBuzzer.IsChanged)
                return geoLocationBuzzer.IsOnString;

            // Check if the desired twin properties contains the geolocationBuzzer
            if (desiredProperties.Contains("geolocationBuzzer"))
            {
                // Get the twin buzzer command
                string buzzer_command = desiredProperties["geolocationBuzzer"];

                Console.WriteLine($"Desired - new telemetry interval: {buzzer_command}");

                // Set the buzzer value according to the command
                App.Repo.Containers[0].Location.BuzzerActuator.SetIsOn(buzzer_command);
            }

            // Since the state of the buzzer actuator was changed according to the tween, set to false
            geoLocationBuzzer.IsChanged = false;

            return geoLocationBuzzer.IsOnString;
        }

        private static string SecurityDoorLockTwin(Twin twin, TwinCollection desiredProperties)
        {
            return "unlock";
        }
        
        private static string PlantsLEDTwin(Twin twin, TwinCollection desiredProperties)
        {
            return "lights-off";
        }
        
        private static string PlantsFANTwin(Twin twin, TwinCollection desiredProperties)
        {
            return "off";
        }
    }
}
