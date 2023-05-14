using ContainerFarm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ContainerFarm.Models.Sensors
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
    /// This class stores the Luminosity sensor information
    /// It has a name property for the name of the sensor and value property.
    /// </summary>
    public class LuminositySensor : ISensor
    {
        public enum Detection
        {
            Detected,
            NotDetected
        };

        public string Name
        {
            get; set;
        }
        public float Value
        {
            get; set;
        }

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
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
