﻿using ContainerFarm.Models.Actuators;
using ContainerFarm.Models.Sensors;
using System.ComponentModel;

namespace ContainerFarm.Models
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
    /// This class stores information related to the security of a container. It stores the 
    /// noise, motion and luminosity detection (detected or not detected) and the status of the door (open or closed).
    /// It also keeps track of the status of the buzzer and door lock of the container
    /// </summary>
    public class Security : INotifyPropertyChanged
    {
        //Enum
        public enum Detection
        {
            Detected,
            NotDetected
        };

        public enum OpenClosed
        {
            Open,
            Closed
        };

        private NoiseSensor noiseSensor;
        private LuminositySensor luminositySensor;
        private MotionSensor motionSensor;
        private DoorSensor doorSensor;
        private DoorlockActuator doorlockActuator;
        private BuzzerActuator buzzerActuator;

        public event PropertyChangedEventHandler PropertyChanged;

        public Security()
        {
            noiseSensor = new NoiseSensor();
            luminositySensor = new LuminositySensor();
            motionSensor = new MotionSensor();
            doorSensor = new DoorSensor();
            doorlockActuator = new DoorlockActuator();
            buzzerActuator = new BuzzerActuator();
        }

        public NoiseSensor NoiseSensor { get { return noiseSensor; } set { noiseSensor = value; } }
        public LuminositySensor LuminositySensor { get { return luminositySensor; }
            set
            {
                luminositySensor = value;
            } }
        public MotionSensor MotionSensor { get { return motionSensor; }
            set
            {
                motionSensor = value;
            } }
        public DoorSensor DoorSensor { get { return doorSensor; }
            set
            {
                doorSensor = value;
            } }
        public DoorlockActuator DoorlockActuator { get { return doorlockActuator; }
            set
            {
                doorlockActuator = value;
            } }
        public BuzzerActuator BuzzerActuator { get { { return buzzerActuator; } } set { buzzerActuator = value; } }




    }
}