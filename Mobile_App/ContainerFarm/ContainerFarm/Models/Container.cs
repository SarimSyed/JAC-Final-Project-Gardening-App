using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
	/// This class stores the name of the container and its geo-location, plant and security 
	/// details (readings from sensors and status of actuators).
    /// </summary>
    public class Container
    {
        public string Name { get; set; }
        public GeoLocation Location { get; set; }
        public Plant Plant { get; set; }
        public Security Security { get; set; }
    }
}
