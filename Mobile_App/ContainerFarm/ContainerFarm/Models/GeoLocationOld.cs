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
    /// This class stores information related to the geo-location of a container. It stores the 
    /// address, pitch angle, roll angle and vibration levels of the container. It also keeps track of the 
    /// status of the buzzer inside the container.
    /// </summary>
    public class GeoLocationOld : INotifyPropertyChanged
    {
        //Private data members
        private string address;
        private double pitchAngle;
        private double rollAngle;
        private double vibrationLevel;
        private bool buzzer;

        /// <summary>
        /// The location(address) of the container.
        /// </summary>
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The pitch angle of the container.
        /// </summary>
        public double PitchAngle
        {
            get { return pitchAngle; }
            set
            {
                pitchAngle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The roll angle of the container.
        /// </summary>
        public double RollAngle
        {
            get { return rollAngle; }
            set
            {
                rollAngle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The vibration levels of the container.
        /// </summary>
        public double VibrationLevel
        {
            get { return vibrationLevel; }
            set
            {
                vibrationLevel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The status of the buzzer (if the buzzer is on) inside the container.
        /// </summary>
        public bool Buzzer
        {
            get { return buzzer; }
            set
            {
                buzzer = value;
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
