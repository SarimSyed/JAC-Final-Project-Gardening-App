using ContainerFarm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ContainerFarm.Models.Actuators
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
    /// This class stores the light actuator information
    /// It has a name property for the actuator name and IsOn porperty for the status of the actuator
    /// </summary>
    public class LightActuator : IActuator
    {
        public string Name
        {
            get; set;
        }
        public bool IsOn
        {
            get; set;
        }

        public bool IsChanged { get; set; }

        public string IsOnString
        {
            get
            {
                return IsOn
                       ? "on"
                       : "off";
            }
        }
        /// <summary>
        /// Sets the IsOn property of the <see cref="FanActuator"/> class based on the specified IsOn string representation.
        /// </summary>
        /// <param name="ledValue">The LED value in string representation (e.g. 'on' or 'off').</param>
        public void SetIsOn(string ledValue)
        {
            IsOn = ledValue == "on"
                 ? true
                 : false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
