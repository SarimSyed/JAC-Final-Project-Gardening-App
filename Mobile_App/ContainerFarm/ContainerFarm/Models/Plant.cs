using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    public class Plant : INotifyPropertyChanged
    {
        private double temperature;
        public double Temperature
        {
            get { return temperature; }
            set { 
                    temperature = value; 
                    OnPropertyChanged(); 
                }
        }

        private double humidity;
        public double Humidity
        {
            get { return humidity; }
            set
            {
                humidity = value;
                OnPropertyChanged();
            }
        }

        private double soilLevel;
        public double SoilLevel
        {
            get { return soilLevel; }
            set
            {
                soilLevel = value;
                OnPropertyChanged();
            }
        }

        private double waterLevel;
        public double WaterLevel
        {
            get { return waterLevel; }
            set
            {
                waterLevel = value;
                OnPropertyChanged();
            }
        }
        private bool light;
        public bool Light
        {
            get { return light; }
            set
            {
                light = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
