using ContainerFarm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models.Sensors
{
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
