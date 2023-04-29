using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Interfaces
{
    interface IActuator : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public bool IsOn { get; set; }

    }
}
