using ContainerFarm.Interfaces;
using System.ComponentModel;
namespace ContainerFarm.Models.Sensors
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
    /// This class stores the soil moisture sensor information
    /// It has a name property for the name of the sensor and value property.
    /// </summary>
    public class SoilMoistureSensor : ISensor
    {
        public string Name
        {
            get; set;
        }
        public float Value
        {
            get; set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
