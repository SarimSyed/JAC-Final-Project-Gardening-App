using ContainerFarm.Interfaces;
using System.ComponentModel;

namespace ContainerFarm.Models.Sensors
{
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
