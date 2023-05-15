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
    /// This class stores the GPS sensor information
    /// It has a name property for the name of the sensor, address property, coordinates property and value property.
    /// </summary>
    public class GpsSensor : ISensor
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Coordinates {get; set;}
        //Will adjust this when the IoT is connected as float has no use in this class
        public float Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
