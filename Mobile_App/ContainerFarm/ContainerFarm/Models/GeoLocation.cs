using ContainerFarm.Models.Actuators;
using ContainerFarm.Models.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
    /// This class stores information related to the geo-location of a container. It stores the 
    /// address, pitch angle, roll angle and vibration levels of the container. It also keeps track of the 
    /// status of the buzzer inside the container.
    /// </summary>
    public class GeoLocation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private GpsSensor gpsSensor;
        private PitchAngleSensor pitchAngleSensor;
        private RollAngleSensor rollAngleSensor;
        private VibrationSensor vibrationSensor;
        private BuzzerActuator buzzerActuator;

        public GeoLocation()
        {
            gpsSensor = new GpsSensor();
            pitchAngleSensor = new PitchAngleSensor();
            rollAngleSensor = new RollAngleSensor();
            vibrationSensor = new VibrationSensor();
            buzzerActuator = new BuzzerActuator();
        }
        public GpsSensor GpsSensor { get { return gpsSensor; }
            set { gpsSensor = value; }
        }
        public PitchAngleSensor PitchAngleSensor { get { return pitchAngleSensor; }
            set
            {
                pitchAngleSensor = value;
            } }
        public RollAngleSensor RollAngleSensor { get { return rollAngleSensor; }
            set
            {
                rollAngleSensor = value;
            } }
        public VibrationSensor VibrationSensor { get { return vibrationSensor; }
            set
            {
                vibrationSensor = value;
            } }
        public BuzzerActuator BuzzerActuator { get {  return buzzerActuator; }
            set
            {
                buzzerActuator = value;
            } }




    }
}
