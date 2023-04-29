using ContainerFarm.Models.Actuators;
using ContainerFarm.Models.Sensors;
using System.ComponentModel;

namespace ContainerFarm.Models
{
    class Security : INotifyPropertyChanged
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

        public NoiseSensor NoiseSensor { get { return noiseSensor; } }
        public LuminositySensor LuminositySensor { get { return luminositySensor; } }
        public MotionSensor MotionSensor { get { return motionSensor; } }
        public DoorSensor DoorSensor { get { return doorSensor; } }
        public DoorlockActuator DoorlockActuator { get { return doorlockActuator; } }
        public BuzzerActuator BuzzerActuator { get { { return buzzerActuator; } } }


    }
}
