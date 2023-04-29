﻿using ContainerFarm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models.Actuators
{
    public class BuzzerActuator : IActuator
    {
        public string Name { get; set; }
        public bool IsOn { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
