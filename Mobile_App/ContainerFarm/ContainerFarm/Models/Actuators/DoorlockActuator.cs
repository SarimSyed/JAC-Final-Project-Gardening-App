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
    /// This class stores the Door lock actuator information
    /// It has a name property for the actuator name and IsOn property for the status of the acturator
    /// </summary>
    public class DoorlockActuator : IActuator
    {
        public string Name { get; set; }

        /// <summary>
        /// Tracks if the actuator was changed in the app.
        /// </summary>
        public bool IsChanged { get; set; }
        public bool IsOn { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string IsOnString
        {
            get
            {
                return IsOn
                       ? "lock"
                       : "unlock";
            }
        }

        /// <summary>
        /// Sets the IsOn property of the <see cref="DoorlockActuator"/> class based on the specified IsOn string representation.
        /// </summary>
        /// <param name="doorLockValue">The door lock value in string representation (e.g. 'unlock' or 'lock').</param>
        public void SetIsOn(string doorLockValue)
        {
            IsOn = doorLockValue == "lock"
                 ? true
                 : false;
        }
    }
}
