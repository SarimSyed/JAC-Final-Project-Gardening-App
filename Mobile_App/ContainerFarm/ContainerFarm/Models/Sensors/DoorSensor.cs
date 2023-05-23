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
    /// This class stores the Door sensor information
    /// It has an OPenClosed enum, name property for the name of the sensor and value property.
    /// It also contains a property for detected to get the value if the door is open or not
    /// </summary>
    public class DoorSensor : ISensor
    {
        public enum OpenClosed
        {
            Open,
            Closed
        };
        public string Name { get; set; }
        public float Value { get; set; }

        public string Detected
        {
            get
            {
                if (Value == 1)
                {
                    return OpenClosed.Closed.ToString();
                }
                else
                {
                    return OpenClosed.Open.ToString();
                }
            }

        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
