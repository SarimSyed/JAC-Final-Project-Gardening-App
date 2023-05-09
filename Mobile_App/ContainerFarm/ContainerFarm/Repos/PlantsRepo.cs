using ContainerFarm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Repos
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III 
    /// <summary>
    /// This class contains the plant test repository used in the farm technician page
    /// </summary>
    public class PlantsRepo
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
