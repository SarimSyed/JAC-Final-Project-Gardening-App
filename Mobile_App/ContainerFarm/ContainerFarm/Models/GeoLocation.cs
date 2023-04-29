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
