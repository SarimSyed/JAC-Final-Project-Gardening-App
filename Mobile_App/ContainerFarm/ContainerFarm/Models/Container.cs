using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    public class Container
    {
		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public GeoLocation GeoLocationDevices { get; set; }
		public Plant PlantDevices { get; set; }
		public Security SecurityDevices { get; set; }

	}
}
