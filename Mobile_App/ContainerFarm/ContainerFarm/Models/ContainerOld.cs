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
    public class ContainerOld
    {
		//Propeties

		/// <summary>
		/// The name of the container.
		/// </summary>
		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

        /// <summary>
        /// The infromation received from the container's geo-location devices.
        /// </summary>
        public GeoLocation GeoLocationDetails { get; set; }

        /// <summary>
        /// The infromation received from the container's plant devices.
        /// </summary>
        public Plant PlantDetails { get; set; }

        /// <summary>
        /// The infromation received from the container's security devices.
        /// </summary>
        public Security SecurityDetails { get; set; }
	}
}
