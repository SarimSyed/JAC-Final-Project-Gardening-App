using ContainerFarm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Repos
{
    internal static class ContainerRepo
    {
        static private ObservableCollection<Container> _containers;

        


        static public ObservableCollection<Container> InitializeRepo()
        {
            _containers = new ObservableCollection<Container>();

            _containers.Add(new Container() { 
                GeoLocationDetails = new GeoLocation() { Address="123 Sesame Street", Buzzer = false, VibrationLevel=0, PitchAngle=2, RollAngle=1}, Name="Schoolyard",
                PlantDetails = new Plant() { Temperature = 20, Fan=false, Humidity = 15, SoilLevel=50, WaterLevel=50, Light=false}, 
                SecurityDetails = new Models.Security() { Buzzer = false, Door = false, DoorLock = false, Luminosity = false, Motion = false, Noise = false }
            });

            return _containers;
        }

    }
}
