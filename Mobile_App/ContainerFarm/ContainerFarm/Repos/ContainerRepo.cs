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
        private const string DOOR = "door";
        private const string DOOR_OPEN = "open";
        private const string DOOR_CLOSE = "close";
        private const string MOTION = "motion";
        private const string MOTION_DETECTED = "detected";
        private const string NOISE = "noise";
        private const string LUMINOSITY = "luminosity";

        private const string WATER_LEVEL = "water-level-sensor";
        private const string SOIL_MOISTURE = "soil-moisture";
        private const string HUMIDITY = "humidity";
        private const string TEMPERATURE = "temperature";
        private const string VALUE = "value";

        private const string BUZZER = "buzzer";
        private const string GPS_ADDRESS = "Address";
        private const string GPS_LOCATION = "gps-location";
        private const string PITCH = "pitch";
        private const string ROLL_ANGLE = "roll-angle";
        private const string VIBRATION = "vibration";


        private ObservableCollection<Container> _containers;

        public ContainerRepo()
        {
            InitializeRepo();
        }

        public ObservableCollection<Container> Containers
        {
            get
            {
                return _containers;
            }
        }

        public void UpdateReadings(string readings)
        {
            JObject sensorJson = JObject.Parse(readings);
            JArray jArray = (JArray)sensorJson["sensors"];

            Console.WriteLine(jArray);

            for (int i = 0; i < jArray.Count; i++)
            {
                JObject oneSensorObject = JObject.Parse(jArray[i].ToString());

                #region Security

                if (oneSensorObject.ToString().Contains(DOOR))
                {
                    string door_value = oneSensorObject[DOOR][VALUE].ToString();

                    if (door_value == DOOR_OPEN)
                        _containers[0].Security.DoorSensor.Value = 0;
                    else
                        _containers[0].Security.DoorSensor.Value = 1;
                }
                else if (oneSensorObject.ToString().Contains(MOTION))
                {
                    string motion_value = oneSensorObject[MOTION][VALUE].ToString();
                    if (motion_value == MOTION_DETECTED)
                        _containers[0].Security.MotionSensor.Value = 1;
                    else
                        _containers[0].Security.MotionSensor.Value = 0;
                }
                else if (oneSensorObject.ToString().Contains(NOISE))
                {
                    string noise_value = oneSensorObject[NOISE][VALUE].ToString();
                    if (Convert.ToInt32(noise_value) <= 100 || Convert.ToInt32(noise_value) > 180)
                        _containers[0].Security.NoiseSensor.Value = 1;
                    else
                        _containers[0].Security.NoiseSensor.Value = 0;
                }
                else if (oneSensorObject.ToString().Contains(LUMINOSITY))
                {
                    string luminosity_value = oneSensorObject[LUMINOSITY][VALUE].ToString();
                    if (Convert.ToInt32(luminosity_value) > 30)
                        _containers[0].Security.LuminositySensor.Value = 1;
                    else
                        _containers[0].Security.LuminositySensor.Value = 0;
                }

                #endregion

                #region Plants

                if (oneSensorObject.ToString().Contains(WATER_LEVEL))
                {
                    string value;
                    value = oneSensorObject[WATER_LEVEL][VALUE].ToString();

                    _containers[0].Plant.WaterLevel.Value = StringToFloat(value);
                }
                if (oneSensorObject.ToString().Contains(SOIL_MOISTURE))
                {
                    string value;

                    value = oneSensorObject[SOIL_MOISTURE][VALUE].ToString();
                    _containers[0].Plant.SoilMoisture.Value = StringToFloat(value);
                }
                if (oneSensorObject.ToString().Contains(HUMIDITY))
                {
                    string value;

                    value = oneSensorObject[HUMIDITY][VALUE].ToString();
                    _containers[0].Plant.Humidity.Value = StringToFloat(value);
                }
                if (oneSensorObject.ToString().Contains(TEMPERATURE))
                {
                    string value;

                    value = oneSensorObject[TEMPERATURE][VALUE].ToString();
                    _containers[0].Plant.Temperature.Value = StringToFloat(value);


                }

                #endregion

                #region Geo Location

                if (oneSensorObject.ToString().Contains(GPS_LOCATION))
                {
                    string gps_location_value = oneSensorObject[GPS_LOCATION][VALUE].ToString();

                    if (!gps_location_value.ToString().Contains(GPS_ADDRESS))
                        return;

                    string[] splitByAddressWord = gps_location_value.Split($"{GPS_ADDRESS}:");

                    string gps_address = splitByAddressWord[1].Trim();
                    _containers[0].Location.GpsSensor.Address = gps_address;                    
                }
                if (oneSensorObject.ToString().Contains(PITCH))
                {
                    string pitch_value = oneSensorObject[PITCH][VALUE].ToString();
                    _containers[0].Location.PitchAngleSensor.Value = Single.Parse(pitch_value, CultureInfo.InvariantCulture);
                }
                if (oneSensorObject.ToString().Contains(ROLL_ANGLE))
                {
                    string roll_angle_value = oneSensorObject[ROLL_ANGLE][VALUE].ToString();
                    _containers[0].Location.RollAngleSensor.Value = Single.Parse(roll_angle_value, CultureInfo.InvariantCulture);
                }
                if (oneSensorObject.ToString().Contains(VIBRATION))
                {
                    string vibration_value = oneSensorObject[VIBRATION][VALUE].ToString();
                    _containers[0].Location.VibrationSensor.Value = Single.Parse(vibration_value, CultureInfo.InvariantCulture);
                }

                #endregion
            }
        }
        private float StringToFloat(string value)
        {

            if (float.TryParse(value, out float parsedValue))
            {
                return parsedValue;
            }
            return 0;
        }

        public void InitializeRepo()
        {
            _containers = new ObservableCollection<Container>();

            //_containers.Add(new Container()
            //{
            //    GeoLocationDetails = new GeoLocation() { Address = "123 Sesame Street", Buzzer = false, VibrationLevel = 0, PitchAngle = 2, RollAngle = 1 },
            //    Name = "Schoolyard",
            //    PlantDetails = new Plant() { Temperature = 20, Fan = false, Humidity = 15, SoilLevel = 50, WaterLevel = 50, Light = false },
            //    SecurityDetails = new Models.Security() { Buzzer = false, Door = Models.Security.OpenClosed.Open, DoorLock = false, Luminosity = Models.Security.Detection.Detected, Motion = Models.Security.Detection.NotDetected, Noise = Models.Security.Detection.Detected }
            //});
            _containers.Add(new Container()
            {
                Name = "Container 1",
                Plant = new Plant()
                {
                    Humidity = new HumiditySensor() { Name = "AH20", Value = 25 },
                    LightActuator = new LightActuator() { IsOn = true, Name = "Light" },
                    SoilMoisture = new SoilMoistureSensor() { Value = 50, Name = "Moisture Sensor" },
                    Temperature = new TemperatureSensor() { Name = "AH20", Value = 15 },
                    WaterLevel = new WaterLevelSensor() { Name = "Water Level", Value = 10 }
                },
                Location = new GeoLocation()
                {
                    BuzzerActuator = new BuzzerActuator() { IsOn = false, Name = "Buzzer" },
                    GpsSensor = new GpsSensor() { Address = "123 Sesame street", Coordinates = "1.1.1.1.1.1", Name = "Sesame Street", Value = 0 },
                    PitchAngleSensor = new PitchAngleSensor() { Value = 10, Name = "Acelerometer" },
                    RollAngleSensor = new RollAngleSensor() { Name = "Acelerometer", Value = 1 },
                    VibrationSensor = new VibrationSensor() { Name = "Vibration Sensor", Value = 0 },
                },
                Security = new Models.Security()
                {
                    BuzzerActuator = new BuzzerActuator() { Name = "Buzzer", IsOn = true },
                    DoorSensor = new DoorSensor() { Name = "Door Sensor", Value = 0 },
                    DoorlockActuator = new DoorlockActuator() { Name = "Door Lock", IsOn = true },
                    LuminositySensor = new LuminositySensor() { Name = "Light Sensor", Value = 1 },
                    MotionSensor = new MotionSensor() { Name = "Motion Sensor", Value = 0 },
                    NoiseSensor = new NoiseSensor() { Value = 0, Name = "Noise Sensor" },
                }

            }); _containers.Add(new Container()
            {
                Name = "Container 2",
                Plant = new Plant()
                {
                    Humidity = new HumiditySensor() { Name = "AH20", Value = 10 },
                    LightActuator = new LightActuator() { IsOn = true, Name = "Light" },
                    SoilMoisture = new SoilMoistureSensor() { Value = 50, Name = "Moisture Sensor" },
                    Temperature = new TemperatureSensor() { Name = "AH20", Value = 50 },
                    WaterLevel = new WaterLevelSensor() { Name = "Water Level", Value = 11 }
                },
                Location = new GeoLocation()
                {
                    BuzzerActuator = new BuzzerActuator() { IsOn = false, Name = "Buzzer" },
                    GpsSensor = new GpsSensor() { Address = "John Abbott", Coordinates = "1.1.1.1.1.1", Name = "Sesame Street", Value = 0 },
                    PitchAngleSensor = new PitchAngleSensor() { Value = 4, Name = "Acelerometer" },
                    RollAngleSensor = new RollAngleSensor() { Name = "Acelerometer", Value = 3 },
                    VibrationSensor = new VibrationSensor() { Name = "Vibration Sensor", Value = 5 },
                },
                Security = new Models.Security()
                {
                    BuzzerActuator = new BuzzerActuator() { Name = "Buzzer", IsOn = false },
                    DoorSensor = new DoorSensor() { Name = "Door Sensor", Value = 0 },
                    DoorlockActuator = new DoorlockActuator() { Name = "Door Lock", IsOn = true },
                    LuminositySensor = new LuminositySensor() { Name = "Light Sensor", Value = 1 },
                    MotionSensor = new MotionSensor() { Name = "Motion Sensor", Value = 1 },
                    NoiseSensor = new NoiseSensor() { Value = 1, Name = "Noise Sensor" },
                }

            });


        }

    }
}
