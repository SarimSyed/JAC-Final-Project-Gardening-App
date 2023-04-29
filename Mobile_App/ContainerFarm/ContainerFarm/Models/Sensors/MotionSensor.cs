using ContainerFarm.Interfaces;
using System.ComponentModel;

namespace ContainerFarm.Models.Sensors
{
    class MotionSensor : ISensor
    {
        public enum Detection
        {
            Detected,
            NotDetected
        };

        public string Name { get; set; }
        public float Value { get; set; }
        public string Motion
        {
            get
            {
                if (Value == 1)
                {
                    return Detection.Detected.ToString();
                }
                else if (Value == 0)
                {
                    return Detection.NotDetected.ToString();

                };
                return Detection.Detected.ToString();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
