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
        public bool IsOn { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
