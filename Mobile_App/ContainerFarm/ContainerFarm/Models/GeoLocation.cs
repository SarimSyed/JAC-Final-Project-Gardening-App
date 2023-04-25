using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    public class GeoLocation : INotifyPropertyChanged
    {
        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }

        private double pitchAngle;
        public double PitchAngle
        {
            get { return pitchAngle; }
            set
            {
                pitchAngle = value;
                OnPropertyChanged();
            }
        }

        private double rollAngle;
        public double RollAngle
        {
            get { return rollAngle; }
            set
            {
                rollAngle = value;
                OnPropertyChanged();
            }
        }

        private double vibrationLevel;
        public double VibrationLevel
        {
            get { return vibrationLevel; }
            set
            {
                vibrationLevel = value;
                OnPropertyChanged();
            }
        }

        private bool buzzer;
        public bool Buzzer
        {
            get { return buzzer; }
            set
            {
                buzzer = value;
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
