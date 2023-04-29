using ContainerFarm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models.Sensors
{
    public class DoorSensor : ISensor
    {
        public enum OpenClosed
        {
            Open,
            Closed
        };
        public string Name { get; set; }
        public float Value { get; set; }

        public string Detected
        {
            get
            {
                if (Value == 1)
                {
                    return OpenClosed.Closed.ToString();
                }
                else
                {
                    return OpenClosed.Open.ToString();
                }
            }

        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
