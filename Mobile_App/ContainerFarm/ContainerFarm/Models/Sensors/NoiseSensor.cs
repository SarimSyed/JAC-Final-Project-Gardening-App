using ContainerFarm.Enums;
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
            Low,
            Medium,
            High,
            NotDetected
        };
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public float Value { get; set; }
        public string Detected
        {
            get
            {
                //Detection
                if (Value <= SecurityReadingTitle.NOISE_LOW_THRESHOLD_HIGH || Value > SecurityReadingTitle.NOISE_HIGH_THRESHOLD_HIGH)
                    return string.Format($"{Detection.High} detection: {Value} dB");

                else if (Value <= SecurityReadingTitle.NOISE_LOW_THRESHOLD_MEDIUM || Value > SecurityReadingTitle.NOISE_HIGH_THRESHOLD_MEDIUM)
                    return string.Format($"{Detection.Medium} detection: {Value} dB");

                else if (Value <= SecurityReadingTitle.NOISE_LOW_THRESHOLD_LOW || Value > SecurityReadingTitle.NOISE_HIGH_THRESHOLD_LOW)
                    return string.Format($"{Detection.Low} detection: {Value} dB");

                else
                    return Detection.NotDetected.ToString();
            }
        }
    }
}
