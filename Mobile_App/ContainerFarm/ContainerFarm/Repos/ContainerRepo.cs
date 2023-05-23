using ContainerFarm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContainerFarm.Models.Sensors;
using ContainerFarm.Models.Actuators;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using ContainerFarm.Enums;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Amqp.Framing;

namespace ContainerFarm.Repos
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
    /// This class contains a test repository for the containers used in the fleet owner views.
    /// </summary>
    internal class ContainerRepo
    {
        private const string VALUE_KEY = "value";
        private const string UNIT_KEY = "unit";

        private ObservableCollection<Container> _containers;

        /// <summary>
        /// Instantiates a new instance of the <see cref="ContainerRepo"/> class.
        /// </summary>
        public ContainerRepo()
        {
            InitializeRepo();
        }

        /// <summary>
        /// The containers list.
        /// </summary>
        public ObservableCollection<Container> Containers { get { return _containers; } }

        /// <summary>
        /// The list of humidity values for the graph representation.
        /// </summary>
        public static ObservableCollection<TempHumiGraphValue> HumidityValues { get; set; } = new ObservableCollection<TempHumiGraphValue>();
        /// <summary>
        /// The list of temperature values for the graph representation.
        /// </summary>
        public static ObservableCollection<TempHumiGraphValue> TemperatureValues { get; set; } = new ObservableCollection<TempHumiGraphValue>();

        /// <summary>
        /// Updates the temperature and humidity list values for the graph representations.
        /// </summary>
        /// <param name="oneSensorObject">The specified sensor object.</param>
        /// <param name="data">The partition event data.</param>
        public void UpdateTemperatureHumidityGraphValues(JObject oneSensorObject, EventData data)
        {
            // Humidity sensor
            if (oneSensorObject.ToString().Contains(PlantReadingTitle.HUMIDITY))
            {
                string unit_value = oneSensorObject[PlantReadingTitle.HUMIDITY][UNIT_KEY].ToString();
                string humidityValueString = oneSensorObject[PlantReadingTitle.HUMIDITY][VALUE_KEY].ToString();
                double humidityValue = StringToFloat(humidityValueString);

                HumidityValues.Add(new TempHumiGraphValue()
                {
                    EnqueuedTime = data.EnqueuedTime,
                    Value = humidityValue,
                    Unit = unit_value,
                });
            }
            // Temperature sensor
            if (oneSensorObject.ToString().Contains(PlantReadingTitle.TEMPERATURE))
            {
                string unit_value = oneSensorObject[PlantReadingTitle.TEMPERATURE][UNIT_KEY].ToString();
                string temperatureValueString = oneSensorObject[PlantReadingTitle.TEMPERATURE][VALUE_KEY].ToString();
                double temperatureValue = StringToFloat(temperatureValueString);

                TemperatureValues.Add(new TempHumiGraphValue()
                {
                    EnqueuedTime = data.EnqueuedTime,
                    Value = temperatureValue,
                    Unit = unit_value,
                });
            }
        }

        /// <summary>
        /// Updates the container subsystem readings.
        /// </summary>
        /// <param name="readings"></param>
        public void UpdateReadings(string readings, EventData data)
        {
            try
            {
                // Get the json object of the readings
                JObject sensorJson = JObject.Parse(readings);
                // Get the array for the 'sensors' property.
                JArray jArray = (JArray)sensorJson["sensors"];

                // DeviceTwinLoop over the array
                for (int i = 0; i < jArray.Count; i++)
                {
                    // Get the sensor object in the current array
                    JObject oneSensorObject = JObject.Parse(jArray[i].ToString());

                    UpdateTemperatureHumidityGraphValues(oneSensorObject, data);

                    UpdateSecurityReading(oneSensorObject);
                    UpdatePlantReading(oneSensorObject);
                    UpdateGeoLocationReading(oneSensorObject);
                }

                _containers[0].Security.GetIssuesCount();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Security Reading

        /// <summary>
        /// Updates the Security reading based on the specified object.
        /// </summary>
        /// <param name="oneSensorObject">The specified sensor object.</param>
        private void UpdateSecurityReading(JObject oneSensorObject)
        {
            // Door sensor 
            if (oneSensorObject.ToString().Contains(SecurityReadingTitle.DOOR))
            {
                string door_value = oneSensorObject[SecurityReadingTitle.DOOR][VALUE_KEY].ToString();
                _containers[0].Security.DoorSensor.Value = door_value == SecurityReadingTitle.DOOR_OPEN
                                                         ? SecurityReadingTitle.TURN_OFF
                                                         : SecurityReadingTitle.TURN_ON;
            }
            // Motion sensor
            else if (oneSensorObject.ToString().Contains(SecurityReadingTitle.MOTION))
            {
                string motion_value = oneSensorObject[SecurityReadingTitle.MOTION][VALUE_KEY].ToString();
                _containers[0].Security.MotionSensor.Value = motion_value == SecurityReadingTitle.MOTION_DETECTED
                                                           ? SecurityReadingTitle.TURN_ON
                                                           : SecurityReadingTitle.TURN_OFF;
            }
            // Noise sensor
            else if (oneSensorObject.ToString().Contains(SecurityReadingTitle.NOISE))
            {
                string noise_value = oneSensorObject[SecurityReadingTitle.NOISE][VALUE_KEY].ToString();
                _containers[0].Security.NoiseSensor.Value = Convert.ToInt32(noise_value);
            }
            // Luminosity sensor
            else if (oneSensorObject.ToString().Contains(SecurityReadingTitle.LUMINOSITY))
            {
                string luminosity_value = oneSensorObject[SecurityReadingTitle.LUMINOSITY][VALUE_KEY].ToString();
                _containers[0].Security.LuminositySensor.Value = Convert.ToInt32(luminosity_value);
            }
        }

        #endregion

        #region Plants Reading

        /// <summary>
        /// Updates the Plant reading based on the specified object.
        /// </summary>
        /// <param name="oneSensorObject">The specified sensor object.</param>
        private void UpdatePlantReading(JObject oneSensorObject)
        {
            // Water level sensor
            if (oneSensorObject.ToString().Contains(PlantReadingTitle.WATER_LEVEL))
            {
                string waterLevelValue = oneSensorObject[PlantReadingTitle.WATER_LEVEL][VALUE_KEY].ToString();
                _containers[0].Plant.WaterLevel.Value = StringToFloat(waterLevelValue);
            }
            // Soil moisture sensor
            else if (oneSensorObject.ToString().Contains(PlantReadingTitle.SOIL_MOISTURE))
            {
                string soilMoistureValue = oneSensorObject[PlantReadingTitle.SOIL_MOISTURE][VALUE_KEY].ToString();
                _containers[0].Plant.SoilMoisture.Value = StringToFloat(soilMoistureValue);
            }
            // Humidity sensor
            else if (oneSensorObject.ToString().Contains(PlantReadingTitle.HUMIDITY))
            {
                string humidityValue = oneSensorObject[PlantReadingTitle.HUMIDITY][VALUE_KEY].ToString();
                _containers[0].Plant.Humidity.Value = StringToFloat(humidityValue);
            }
            // Temperature sensor
            else if (oneSensorObject.ToString().Contains(PlantReadingTitle.TEMPERATURE))
            {
                string temperatureValue = oneSensorObject[PlantReadingTitle.TEMPERATURE][VALUE_KEY].ToString();
                _containers[0].Plant.Temperature.Value = StringToFloat(temperatureValue);
            }
        }

        #endregion
        
        #region Geo Location Reading

        /// <summary>
        /// Updates the Geo Location reading based on the specified object.
        /// </summary>
        /// <param name="oneSensorObject">The specified sensor object.</param>
        private void UpdateGeoLocationReading(JObject oneSensorObject)
        {
            // GPS address location sensor
            if (oneSensorObject.ToString().Contains(GeoLocationReadingTitle.GPS_LOCATION))
            {
                string gps_location_value = oneSensorObject[GeoLocationReadingTitle.GPS_LOCATION][VALUE_KEY].ToString();

                // If no 'Address' word in gps location, no address location
                if (!gps_location_value.ToString().Contains(GeoLocationReadingTitle.GPS_ADDRESS))
                    return;

                // Split to get the address location
                string[] splitByAddressWord = gps_location_value.Split($"{GeoLocationReadingTitle.GPS_ADDRESS}:");

                // Get the address location 
                string gps_address = splitByAddressWord[1].Trim();

                _containers[0].Location.GpsSensor.Address = gps_address;
            }
            // Pitch sensor
            else if (oneSensorObject.ToString().Contains(GeoLocationReadingTitle.PITCH))
            {
                string pitch_value = oneSensorObject[GeoLocationReadingTitle.PITCH][VALUE_KEY].ToString();
                _containers[0].Location.PitchAngleSensor.Value = Single.Parse(pitch_value, CultureInfo.InvariantCulture);
            }
            // Roll angle sensor
            else if (oneSensorObject.ToString().Contains(GeoLocationReadingTitle.ROLL_ANGLE))
            {
                string roll_angle_value = oneSensorObject[GeoLocationReadingTitle.ROLL_ANGLE][VALUE_KEY].ToString();
                _containers[0].Location.RollAngleSensor.Value = Single.Parse(roll_angle_value, CultureInfo.InvariantCulture);
            }
            // Vibration sensor
            else if (oneSensorObject.ToString().Contains(GeoLocationReadingTitle.VIBRATION))
            {
                string vibration_value = oneSensorObject[GeoLocationReadingTitle.VIBRATION][VALUE_KEY].ToString();
                _containers[0].Location.VibrationSensor.Value = Single.Parse(vibration_value, CultureInfo.InvariantCulture);
            }
        }

        #endregion

        /// <summary>
        /// Converts the specified string to a float.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The float waterLevelValue if could parse, otherwise 0.</returns>
        private float StringToFloat(string value)
        {
            if (float.TryParse(value, out float parsedValue))
                return parsedValue;
            return 0;
        }

        /// <summary>
        /// Initializes the containers repo with default containers.
        /// </summary>
        public void InitializeRepo()
        {
            _containers = new ObservableCollection<Container>();

            _containers.Add(new Container()
            {
                Name = "Container 1",
                Plant = new Plant()
                {
                    Humidity = new HumiditySensor() { Name = "AH20", Value = 25 },
                    LightActuator = new LightActuator() { IsOn = false, Name = "Light" },
                    SoilMoisture = new SoilMoistureSensor() { Value = 50, Name = "Moisture Sensor" },
                    Temperature = new TemperatureSensor() { Name = "AH20", Value = 15 },
                    WaterLevel = new WaterLevelSensor() { Name = "Water Level", Value = 10 }
                },
                Location = new GeoLocation()
                {
                    BuzzerActuator = new BuzzerActuator() { IsOn = false, Name = "Buzzer" },
                    GpsSensor = new GpsSensor() { Address = "21 275 Rue Lakeshore Road, Sainte-Anne-de-Bellevue, QC H9X 3L9", Coordinates = "1.1.1.1.1.1", Name = "John Abbott College", Value = 0 },
                    PitchAngleSensor = new PitchAngleSensor() { Value = 10, Name = "Acelerometer" },
                    RollAngleSensor = new RollAngleSensor() { Name = "Acelerometer", Value = 1 },
                    VibrationSensor = new VibrationSensor() { Name = "Vibration Sensor", Value = 0 },
                },
                Security = new Models.Security()
                {
                    BuzzerActuator = new BuzzerActuator() { Name = "Buzzer", IsOn = false },
                    DoorSensor = new DoorSensor() { Name = "Door Sensor", Value = 0 },
                    DoorlockActuator = new DoorlockActuator() { Name = "Door Lock", IsOn = false },
                    LuminositySensor = new LuminositySensor() { Name = "Light Sensor", Value = 1 },
                    MotionSensor = new MotionSensor() { Name = "Motion Sensor", Value = 0 },
                    NoiseSensor = new NoiseSensor() { Value = 0, Name = "Noise Sensor" },
                }

                //}); _containers.Add(new Container()
                //{
                //    Name = "Container 2",
                //    Plant = new Plant()
                //    {
                //        Humidity = new HumiditySensor() { Name = "AH20", Value = 10 },
                //        LightActuator = new LightActuator() { IsOn = true, Name = "Light" },
                //        SoilMoisture = new SoilMoistureSensor() { Value = 50, Name = "Moisture Sensor" },
                //        Temperature = new TemperatureSensor() { Name = "AH20", Value = 50 },
                //        WaterLevel = new WaterLevelSensor() { Name = "Water Level", Value = 11 }
                //    },
                //    Location = new GeoLocation()
                //    {
                //        BuzzerActuator = new BuzzerActuator() { IsOn = false, Name = "Buzzer" },
                //        GpsSensor = new GpsSensor() { Address = "John Abbott", Coordinates = "1.1.1.1.1.1", Name = "Sesame Street", Value = 0 },
                //        PitchAngleSensor = new PitchAngleSensor() { Value = 4, Name = "Acelerometer" },
                //        RollAngleSensor = new RollAngleSensor() { Name = "Acelerometer", Value = 3 },
                //        VibrationSensor = new VibrationSensor() { Name = "Vibration Sensor", Value = 5 },
                //    },
                //    Security = new Models.Security()
                //    {
                //        BuzzerActuator = new BuzzerActuator() { Name = "Buzzer", IsOn = false },
                //        DoorSensor = new DoorSensor() { Name = "Door Sensor", Value = 0 },
                //        DoorlockActuator = new DoorlockActuator() { Name = "Door Lock", IsOn = true },
                //        LuminositySensor = new LuminositySensor() { Name = "Light Sensor", Value = 1 },
                //        MotionSensor = new MotionSensor() { Name = "Motion Sensor", Value = 1 },
                //        NoiseSensor = new NoiseSensor() { Value = 1, Name = "Noise Sensor" },
                //    }

            });
        }
    }
}
