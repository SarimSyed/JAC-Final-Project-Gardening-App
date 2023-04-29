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
    /// This class stores information related to the security of a container. It stores the 
    /// noise, motion and luminosity detection (detected or not detected) and the status of the door (open or closed).
    /// It also keeps track of the status of the buzzer and door lock of the container
    /// </summary>
    public class SecurityOld : INotifyPropertyChanged
    {
        //Enum
        public enum Detection
        {
            Detected,
            NotDetected
        };

        public enum OpenClosed
        {
            Open,
            Closed
        };

        //Private data members
        private Detection noise;
        private Detection luminosity;
        private Detection motion;
        private OpenClosed door;
        private bool doorLock;
        private bool buzzer;

        /// <summary>
        /// Noise detection inside the container (if noise is detected).
        /// </summary>
        public Detection Noise
        {
            get { return noise; }
            set
            {
                noise = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Luminosity detection inside the container (if light is detected).
        /// </summary>
        public Detection Luminosity
        {
            get { return luminosity; }
            set
            {
                luminosity = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Motion detection inside the container (if movement is detected).
        /// </summary>
        public Detection Motion
        {
            get { return motion; }
            set
            {
                motion = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The status of the door of the container (if the door is open).
        /// </summary>
        public OpenClosed Door
        {
            get { return door; }
            set
            {
                door = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The status of the door lock of the container (if the door is locked)
        /// </summary>
        public bool DoorLock
        {
            get { return doorLock; }
            set
            {
                doorLock = value;
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
