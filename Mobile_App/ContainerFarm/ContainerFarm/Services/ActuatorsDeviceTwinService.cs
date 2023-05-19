using ContainerFarm.Enums;
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

            try
            {
                TwinCollection desiredProperties = twin.Properties.Desired;
                TwinCollection reportedProperties = twin.Properties.Reported;

                // Get the property values to updated the desired properties
                string geolocationBuzzer = await GeoLocationBuzzerTwin(twin, desiredProperties, reportedProperties);
                string securityDoorLock = SecurityDoorLockTwin(twin, desiredProperties, reportedProperties);
                string plantsLED = PlantsLEDTwin(twin, desiredProperties, reportedProperties);
                string plantsFAN = await PlantsFANTwinAsync(twin, desiredProperties, reportedProperties);

                // Create the new twin patch
                var patch =
                    $@"{{
                    properties: {{
                        desired: {{
                            ""{GeoLocationTwinProperties.BUZZER}"": ""{geolocationBuzzer}"",
                            ""{SecurityTwinProperties.DOOR_LOCK}"": ""{securityDoorLock}"",
                            ""{PlantsTwinProperties.LED}"": ""{plantsLED}"",
                            ""{PlantsTwinProperties.FAN}"": ""{plantsFAN}"",
                        }}
                    }}
                }}";

                // Update the device twin with the new patch
                await RegistryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);

                Console.WriteLine(twin.Properties.Reported);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the GeoLocation buzzer actuator from the specified twin desired properties.
        /// </summary>
        /// <param name="desiredProperties">The desired properties of the device twin.</param>
        /// <returns>The GeoLocation buzzer actuator twin property value.</returns>
        private static async Task<string> GeoLocationBuzzerTwin(Twin twin, TwinCollection desiredProperties, TwinCollection reportedProperties)
        {
            try
            {
                // Get the Geo Location buzzer actuator from the container
                BuzzerActuator geoLocationBuzzer = App.Repo.Containers[0].Location.BuzzerActuator;
                BuzzerActuator securityBuzzer = App.Repo.Containers[0].Security.BuzzerActuator;

                // Don't check twin properties if the actuator was just set to 'on' in the app
                if (geoLocationBuzzer.IsChanged)
                {
                    // Since the state of the buzzer actuator was changed according to the twin, set to false
                    geoLocationBuzzer.IsChanged = false;

                    return geoLocationBuzzer.IsOnString;
                }

                // Don't check twin properties if the actuator was just set to 'on' in the app
                if (securityBuzzer.IsChanged)
                {
                    // Since the state of the buzzer actuator was changed according to the twin, set to false
                    securityBuzzer.IsChanged = false;

                    return securityBuzzer.IsOnString;
                }

                // Check if the desired twin properties contains the geolocationBuzzer
                if (desiredProperties.Contains(GeoLocationTwinProperties.BUZZER))
                {
                    // Get the twin buzzer command
                    string buzzer_command = (string) desiredProperties[GeoLocationTwinProperties.BUZZER];

                    Console.WriteLine($"Desired - new {GeoLocationTwinProperties.BUZZER} command: {buzzer_command}");

                    // Set the buzzer value according to the command
                    App.Repo.Containers[0].Location.BuzzerActuator.SetIsOn(buzzer_command);
                    App.Repo.Containers[0].Security.BuzzerActuator.SetIsOn(buzzer_command);
                }

                // Check if the reported twin properties contains the geolocationBuzzer
                if (reportedProperties.Contains(GeoLocationTwinProperties.BUZZER))
                {
                    // Get the twin buzzer command
                    string buzzer_command = (string) reportedProperties[GeoLocationTwinProperties.BUZZER];

                    Console.WriteLine($"Reported - new {GeoLocationTwinProperties.BUZZER} command: {buzzer_command}");

                    // Set the buzzer switch value according to if the buzzer was actually turned on
                    if ((App.Repo.Containers[0].Location.BuzzerActuator.IsOn && App.Repo.Containers[0].Security.BuzzerActuator.IsOn) && buzzer_command == "off")
                    {
                        App.Repo.Containers[0].Location.BuzzerActuator.IsOn = false;
                        App.Repo.Containers[0].Security.BuzzerActuator.IsOn = false;

                        Console.WriteLine("buzzer turned off");
                        //await Application.Current.MainPage.DisplayAlert("Buzzer turned off", "The buzzer wasn't turned on successfully. Please check if the container farm is running.", "OK");
                    }
                }

                return geoLocationBuzzer.IsOnString;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string SecurityDoorLockTwin(Twin twin, TwinCollection desiredProperties, TwinCollection reportedProperties)
        {
            return "unlock";
        }
        
        private static string PlantsLEDTwin(Twin twin, TwinCollection desiredProperties, TwinCollection reportedProperties)
        {
            return "lights-off";
        }
        
        private static async Task<string> PlantsFANTwinAsync(Twin twin, TwinCollection desiredProperties, TwinCollection reportedProperties)
        {
            try
            {
                // Get the Fan actuator from the container
                FanActuator plantsFan = App.Repo.Containers[0].Plant.FanActuator;

                // Don't check twin properties if the actuator was just set to 'on' in the app
                if (plantsFan.IsChanged)
                {
                    // Since the state of the fan actuator was changed according to the twin, set to false
                    plantsFan.IsChanged = false;

                    return plantsFan.IsOnString;
                }

                // Check if the desired twin properties contains the plantsFan
                if (desiredProperties.Contains(PlantsTwinProperties.FAN))
                {
                    // Get the twin fan command
                    string fan_command = (string) desiredProperties[PlantsTwinProperties.FAN];

                    Console.WriteLine($"Desired - new {PlantsTwinProperties.FAN} command: {fan_command}");

                    // Set the fan value according to the command
                    App.Repo.Containers[0].Plant.FanActuator.SetIsOn(fan_command);
                }

                // Check if the reported twin properties contains the plantsFan
                if (reportedProperties.Contains(PlantsTwinProperties.FAN))
                {
                    // Get the twin fan command
                    string fan_command = reportedProperties[PlantsTwinProperties.FAN];

                    Console.WriteLine($"Reported - new {PlantsTwinProperties.FAN} command: {fan_command}");

                    // Set the fan switch value according to if the fan was actually turned on
                    if (App.Repo.Containers[0].Plant.FanActuator.IsOn && fan_command == "off")
                    {
                        App.Repo.Containers[0].Plant.FanActuator.IsOn = false;

                        Console.WriteLine("fan turned off");

                        //await Application.Current.MainPage.DisplayAlert("fan turned off", "The fan wasn't turned on successfully. Please check if the container farm is running.", "OK");
                    }
                }

                return plantsFan.IsOnString;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
