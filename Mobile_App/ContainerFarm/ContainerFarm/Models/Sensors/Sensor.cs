using ContainerFarm.Interfaces;
using System.ComponentModel;

namespace ContainerFarm.Models.Sensors
{
    public class Sensor : ISensor
    {
        public string Name { get; set; }
        public float Value { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
