﻿using ContainerFarm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContainerFarm.Models.Sensors;
using ContainerFarm.Models.Actuators;

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

            });            _containers.Add(new Container()
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