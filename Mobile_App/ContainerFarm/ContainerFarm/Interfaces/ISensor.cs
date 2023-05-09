using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Interfaces
{
    interface ISensor : INotifyPropertyChanged
    {
        string Name { get; set; }
        public float Value { get; set; }

    }
}
