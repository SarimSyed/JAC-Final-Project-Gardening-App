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
    /// This class stores the fan actuator information
    /// It has a name property for the actuator name and IsOn property for tthe status of the actuator
    /// </summary>
    public class FanActuator : IActuator
    {
        public string Name { get; set; }
        public bool IsOn { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Tracks if the actuator was changed in the app.
        /// </summary>
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
        /// <param name="fanValue">The buzzer value in string representation (e.g. 'on' or 'off').</param>
        public void SetIsOn(string fanValue)
        {
            IsOn = fanValue == "on"
                 ? true
                 : false;
        }
    }
}
