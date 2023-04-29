using ContainerFarm.Interfaces;
using System.ComponentModel;

namespace ContainerFarm.Models.Sensors
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
    /// This class stores the Noise sensor information
    /// It has a name property for the name of the sensor and value property.
    /// It also contains a detected property that returns if noise has been detected or not
    /// </summary>
    public class NoiseSensor : ISensor
    {
        public enum Detection
        {
            Detected,
            NotDetected
        };
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public float Value { get; set; }
        public string Detected
        {
            get
            {
                if (Value == 0)
                {
                    return Detection.NotDetected.ToString();
                }
                else
                {
                    return Detection.Detected.ToString();
                }
            }
        }
    }
}
