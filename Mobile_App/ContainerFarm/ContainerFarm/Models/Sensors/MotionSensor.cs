using ContainerFarm.Interfaces;
using System.ComponentModel;

namespace ContainerFarm.Models.Sensors
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
    /// This class stores the Motion sensor information
    /// It has a name property for the name of the sensor and value property.
    /// It also contains a detected property that returns if motion has been detected or not
    /// </summary>
    public class MotionSensor : ISensor
    {
        public enum Detection
        {
            Detected,
            NotDetected
        };

        public string Name { get; set; }
        public float Value { get; set; }
        public string Detected
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
