using ContainerFarm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Repos
{
    internal class ContainerRepo
    {
         private ObservableCollection<Container> _containers;

        public ContainerRepo()
        {
            InitializeRepo();
        }

        public ObservableCollection<Container> Containers
        {
            get
            {
                return _containers;
            }
        }

        public void InitializeRepo()
        {
            _containers = new ObservableCollection<Container>();

            _containers.Add(new Container()
            {
                GeoLocationDetails = new GeoLocation() { Address = "123 Sesame Street", Buzzer = false, VibrationLevel = 0, PitchAngle = 2, RollAngle = 1 },
                Name = "Schoolyard",
                PlantDetails = new Plant() { Temperature = 20, Fan = false, Humidity = 15, SoilLevel = 50, WaterLevel = 50, Light = false },
                SecurityDetails = new Models.Security() { Buzzer = false, Door = Models.Security.OpenClosed.Open, DoorLock = false, Luminosity = Models.Security.Detection.Detected, Motion = Models.Security.Detection.NotDetected, Noise = Models.Security.Detection.Detected }
            });


        }

    }
}
