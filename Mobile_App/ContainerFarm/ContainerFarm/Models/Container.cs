using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    public class Container
    {
        public string Name { get; set; }
        public GeoLocation Location { get; set; }
        public Plant Plant { get; set; }
        public Security Security { get; set; }
    }
}
