using ContainerFarm.Models.Actuators;
using ContainerFarm.Models.Sensors;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

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
        private string motionsensor;
        private int issueCount;

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

        public NoiseSensor NoiseSensor { get { return noiseSensor; } set { noiseSensor = value;} }
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
        
        public int IssuesCount
        {
            get { return issueCount; }
            set
            {
                issueCount = value;
            }
        }

        public int GetIssuesCount()
        {
            int issuesCount = 0;
            if (MotionSensor.Detected == MotionSensor.Detection.Detected.ToString())
                issuesCount++;
            if (DoorSensor.Detected == DoorSensor.OpenClosed.Open.ToString())
                issuesCount++;
            if (NoiseSensor.Detected.Contains(NoiseSensor.Detection.High.ToString()))
                issuesCount++;
            if (LuminositySensor.Detected.Contains(LuminositySensor.Detection.VeryBright.ToString()))
                issuesCount++;

            IssuesCount = issuesCount;

            return issuesCount;
        }

        /// <summary>
        /// Builds the list of issue messages.
        /// </summary>
        /// <returns></returns>
        public string IssuesMessage()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (MotionSensor.Detected == MotionSensor.Detection.Detected.ToString())
                stringBuilder.AppendLine("Motion was detected");
            if (DoorSensor.Detected == DoorSensor.OpenClosed.Open.ToString())
                stringBuilder.AppendLine("Door is open");
            if (NoiseSensor.Detected.Contains(NoiseSensor.Detection.High.ToString()))
                stringBuilder.AppendLine("High noise level was detected");
            if (LuminositySensor.Detected.Contains(LuminositySensor.Detection.VeryBright.ToString()))
                stringBuilder.AppendLine("HIgh luminosity level was detected");

            return stringBuilder.ToString();
        }

        public string IssuesUri
        {
            get
            {
                if (IssuesCount == 0)
                    return "accept.png";
                else if (IssuesCount > 3)
                    return "bad.png";
                else
                    return "warning.png";
            }
        }

        /// <summary>
        /// Invokes property changed event for a property whose value changed.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



    }
}
