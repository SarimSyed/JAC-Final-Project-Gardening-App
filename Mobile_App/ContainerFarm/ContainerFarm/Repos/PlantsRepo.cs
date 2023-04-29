using ContainerFarm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Repos
{
    class PlantsRepo
    {
        public PlantOld plant = new PlantOld()
        {
            Temperature = 15.158,
            Humidity = 25,
            SoilLevel = 10,
            WaterLevel = 10,
            Light = true,
            Fan = false
        };
    }
}
