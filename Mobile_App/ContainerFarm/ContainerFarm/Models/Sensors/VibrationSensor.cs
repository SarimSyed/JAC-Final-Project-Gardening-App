using ContainerFarm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ContainerFarm.Models.Sensors
{
    class VibrationSensor : ISensor
    {
        public string Name
        {
            get; set;
        }
        public float Value
        {
            get; set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
