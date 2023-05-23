using ContainerFarm.Enums;
using ContainerFarm.Models;
using ContainerFarm.Models.Actuators;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Services
{
    public static class ActuatorsDeviceTwinService
    {
        public static readonly int SLEEP_LOOP_TIME = 3000;

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

            // Check internet connection
            NetworkAccess networkAccess = Connectivity.Current.NetworkAccess;

            // Throw exception if no internet access
            if (networkAccess != NetworkAccess.Internet)
            {
                TurnOffAllActuatorSwitches();
                throw new AggregateException($"No internet connection. Please connect to the internet.");
            }

            try
            {
                TwinCollection desiredProperties = twin.Properties.Desired;
                TwinCollection reportedProperties = twin.Properties.Reported;

                // Get the property values to updated the desired properties
                string geolocationBuzzer = await GeoLocationBuzzerTwin(desiredProperties, reportedProperties);
                string securityDoorLock = SecurityDoorLockTwin(desiredProperties, reportedProperties);
                string plantsLED = PlantsLEDTwin(desiredProperties, reportedProperties);
                string plantsFAN = await PlantsFANTwinAsync(desiredProperties, reportedProperties);
                int telemetryInterval = TelemtryIntervalTwin(desiredProperties);

                // Create the new twin patch
                var patch =
                    $@"{{
                    properties: {{
                        desired: {{
                            ""{TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY}"": {telemetryInterval},
                            ""{GeoLocationTwinProperties.BUZZER}"": ""{geolocationBuzzer}"",
                            ""{SecurityTwinProperties.DOOR_LOCK}"": ""{securityDoorLock}"",
                            ""{SecurityTwinProperties.BUZZER}"": ""{geolocationBuzzer}"",
                            ""{PlantsTwinProperties.LED}"": ""{plantsLED}"",
                            ""{PlantsTwinProperties.FAN}"": ""{plantsFAN}"",
                        }},
                        reported: {{
                            ""{TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY}"": {telemetryInterval}
                        }}
                    }}
                }}";

                // Update the device twin with the new patch
                await RegistryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);

                Console.WriteLine(twin.Properties.Reported);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the GeoLocation buzzer actuator from the specified twin desired properties.
        /// </summary>
        /// <param name="desiredProperties">The desired properties of the device twin.</param>
        /// <param name="reportedProperties">The desired properties of the device twin.</param>
        /// <returns>The GeoLocation buzzer actuator twin property value.</returns>
        private static async Task<string> GeoLocationBuzzerTwin(TwinCollection desiredProperties, TwinCollection reportedProperties)
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
                    string buzzer_command = (string)desiredProperties[GeoLocationTwinProperties.BUZZER];

                    Console.WriteLine($"Desired - new {GeoLocationTwinProperties.BUZZER} command: {buzzer_command}");

                    // Set the buzzer value according to the command
                    App.Repo.Containers[0].Location.BuzzerActuator.SetIsOn(buzzer_command);
                    App.Repo.Containers[0].Security.BuzzerActuator.SetIsOn(buzzer_command);
                }
                else
                    App.Repo.Containers[0].Location.BuzzerActuator.IsOn = false;

                // Check if the reported twin properties contains the geolocationBuzzer
                if (reportedProperties.Contains(GeoLocationTwinProperties.BUZZER))
                {
                    // Get the twin buzzer command
                    string buzzer_command = (string) reportedProperties[GeoLocationTwinProperties.BUZZER];

                    Console.WriteLine($"Reported - new {GeoLocationTwinProperties.BUZZER} command: {buzzer_command}");

                    // Set the buzzer switch value according to if the buzzer was actually turned on
                    if ((App.Repo.Containers[0].Location.BuzzerActuator.IsOn || App.Repo.Containers[0].Security.BuzzerActuator.IsOn) && buzzer_command == "off")
                    {
                        App.Repo.Containers[0].Location.BuzzerActuator.IsOn = false;
                        App.Repo.Containers[0].Security.BuzzerActuator.IsOn = false;

                        Console.WriteLine("buzzer turned off");
                    }
                }
                else
                    App.Repo.Containers[0].Location.BuzzerActuator.IsOn = false;

                return geoLocationBuzzer.IsOnString;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the Security door lock actuator from the specified twin desired properties.
        /// </summary>
        /// <param name="desiredProperties">The desired properties of the device twin.</param>
        /// <param name="reportedProperties">The desired properties of the device twin.</param>
        /// <returns>The Security door lock actuator twin property value.</returns>
        private static string SecurityDoorLockTwin(TwinCollection desiredProperties, TwinCollection reportedProperties)
        {
            DoorlockActuator doorlock = App.Repo.Containers[0].Security.DoorlockActuator;

            if (doorlock.IsChanged)
            {
                doorlock.IsChanged = false;

                return doorlock.IsOnString;
            }

            //Check if desired properties contains plantsLED
            if (desiredProperties.Contains(SecurityTwinProperties.DOOR_LOCK))
            {
                string door_command = (string) desiredProperties[SecurityTwinProperties.DOOR_LOCK];
                Console.WriteLine($"Desired - new {SecurityTwinProperties.DOOR_LOCK} command: {door_command}");

                //set led value according to cmd
                App.Repo.Containers[0].Security.DoorlockActuator.SetIsOn(door_command);
            }
            else
                App.Repo.Containers[0].Security.DoorlockActuator.IsOn = false;

            //check if reported twin properties contains the plantsLed
            if (reportedProperties.Contains(SecurityTwinProperties.DOOR_LOCK))
            {
                //get twin command
                string door_command = (string) reportedProperties[SecurityTwinProperties.DOOR_LOCK];

                Console.WriteLine($"Reported - new {SecurityTwinProperties.DOOR_LOCK} command: {door_command}");

                //set the led switch value according to if the LED was actually turned on
                if (App.Repo.Containers[0].Security.DoorlockActuator.IsOn && door_command == "unlock")
                    App.Repo.Containers[0].Security.DoorlockActuator.IsOn = false;
            }
            else
                App.Repo.Containers[0].Security.DoorlockActuator.IsOn = false;

            return doorlock.IsOnString;
        }

        /// <summary>
        /// Updates the Plants LED actuator from the specified twin desired properties.
        /// </summary>
        /// <param name="desiredProperties">The desired properties of the device twin.</param>
        /// <param name="reportedProperties">The desired properties of the device twin.</param>
        /// <returns>The Plants LED actuator twin property value.</returns>
        private static string PlantsLEDTwin(TwinCollection desiredProperties, TwinCollection reportedProperties)
        {
            LightActuator plantsLED = App.Repo.Containers[0].Plant.LightActuator;

            if (plantsLED.IsChanged)
            {
                plantsLED.IsChanged = false;

                return plantsLED.IsOnString;
            }

            //Check if desired properties contains plantsLED
            if (desiredProperties.Contains(PlantsTwinProperties.LED))
            {
                string led_command = (string) desiredProperties[PlantsTwinProperties.LED];
                Console.WriteLine($"Desired - new {PlantsTwinProperties.LED} command: {led_command}");

                //set led value according to cmd
                App.Repo.Containers[0].Plant.LightActuator.SetIsOn(led_command);
            }
            else
                App.Repo.Containers[0].Plant.LightActuator.IsOn = false;

            //check if reported twin properties contains the plantsLed
            if (reportedProperties.Contains(PlantsTwinProperties.LED))
            {
                //get twin command
                string led_command = (string) reportedProperties[PlantsTwinProperties.LED];

                Console.WriteLine($"Reported - new {PlantsTwinProperties.LED} command: {led_command}");

                //set the led switch value according to if the LED was actually turned on
                if (App.Repo.Containers[0].Plant.LightActuator.IsOn && (led_command == "off" || led_command == "lights-off"))
                    App.Repo.Containers[0].Plant.LightActuator.IsOn = false;
            }
            else
                App.Repo.Containers[0].Plant.LightActuator.IsOn = false;

            return plantsLED.IsOnString;
        }

        /// <summary>
        /// Updates the Plants FAN actuator from the specified twin desired properties.
        /// </summary>
        /// <param name="desiredProperties">The desired properties of the device twin.</param>
        /// <param name="reportedProperties">The desired properties of the device twin.</param>
        /// <returns>The Plants FAN actuator twin property value.</returns>
        private static async Task<string> PlantsFANTwinAsync(TwinCollection desiredProperties, TwinCollection reportedProperties)
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
                else
                    App.Repo.Containers[0].Plant.FanActuator.IsOn = false;

                // Check if the reported twin properties contains the plantsFan
                if (reportedProperties.Contains(PlantsTwinProperties.FAN))
                {
                    // Get the twin fan command
                    string fan_command = (string) reportedProperties[PlantsTwinProperties.FAN];

                    Console.WriteLine($"Reported - new {PlantsTwinProperties.FAN} command: {fan_command}");

                    // Set the fan switch value according to if the fan was actually turned on
                    if (App.Repo.Containers[0].Plant.FanActuator.IsOn && fan_command == "off")
                    {
                        App.Repo.Containers[0].Plant.FanActuator.IsOn = false;

                        Console.WriteLine("fan turned off");
                    }
                }
                else
                    App.Repo.Containers[0].Plant.FanActuator.IsOn = false;

                return plantsFan.IsOnString;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the Telemetry Interval from the specified twin desired properties.
        /// </summary>
        /// <param name="desiredProperties">The desired properties of the device twin.</param>
        /// <returns>The Telemetry Interval.</returns>
        private static int TelemtryIntervalTwin(TwinCollection desiredProperties)
        {
            // If the telemetry interval was changes in the settings app
            if (TelemetryIntervalModel.IsChanged)
            {
                TelemetryIntervalModel.IsChanged = false;
                return TelemetryIntervalModel.TelemetryInterval;
            }

            try
            {
                // Check if the desired twin properties contains the telemetryInterval
                if (desiredProperties.Contains(TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY))
                {
                    // Get the twin telemetryInterval command
                    int new_telemetry_interval = (int) desiredProperties[TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY];

                    Console.WriteLine($"Desired - new {TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY} command: {new_telemetry_interval}");

                    // Set the telemetryInterval value according to the command
                    TelemetryIntervalModel.SetTelemetryInterval(new_telemetry_interval);

                    // Update Preferences
                    PreferencesService.UpdateSpecificPreference(TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY, new_telemetry_interval);
                }

                return TelemetryIntervalModel.TelemetryInterval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Turns off all actuators in the app.
        /// </summary>
        private static void TurnOffAllActuatorSwitches()
        {
            App.Repo.Containers[0].Location.BuzzerActuator.IsOn = false;
            App.Repo.Containers[0].Security.BuzzerActuator.IsOn = false;
            App.Repo.Containers[0].Security.DoorlockActuator.IsOn = false;
            App.Repo.Containers[0].Plant.LightActuator.IsOn = false;
            App.Repo.Containers[0].Plant.FanActuator.IsOn = false;
        }
    }
}
