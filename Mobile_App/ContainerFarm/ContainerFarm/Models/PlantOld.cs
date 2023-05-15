using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III
    /// <summary>
    /// This class stores information related to the plants inside a container. It stores the 
    /// temperature and humidity of the container and water level and soil level of the plants. 
    /// It also keeps track of the status of the fan and light inside the container.
    /// </summary>
    public class PlantOld : INotifyPropertyChanged
    {
        //Private data members
        private double temperature;
        private double humidity;
        private double soilLevel;
        private double waterLevel;
        private bool light;
        private bool fan;

        /// <summary>
        /// The temperature value inside the container.
        /// </summary>
        public double Temperature
        {
            get { return temperature; }
            set { 
                    temperature = value; 
                    OnPropertyChanged(); 
                }
        }

        /// <summary>
        /// The humidity percentage inside the container.
        /// </summary>
        public double Humidity
        {
            get { return humidity; }
            set
            {
                humidity = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The soil level of the plants inside the container.
        /// </summary>
        public double SoilLevel
        {
            get { return soilLevel; }
            set
            {
                soilLevel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The water level of the plants inside the container.
        /// </summary>
        public double WaterLevel
        {
            get { return waterLevel; }
            set
            {
                waterLevel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The status of the light (if the light is on) inside the container.
        /// </summary>
        public bool Light
        {
            get { return light; }
            set
            {
                light = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The status of the fan (if the fan is on) inside the container.
        /// </summary>
        public bool Fan
        {
            get { return fan; }
            set
            {
                fan = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes property changed event for a property whose value changed.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
