using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    public class Security :INotifyPropertyChanged
    {
        public enum DetectionType
        {
            Detected,
            NotDetected
        }
        public enum DoorStatus
        {
            Open,
            Closed
        }

        private DetectionType noise;
        public DetectionType Noise
        {
            get { return noise; }
            set
            {
                noise = value;
                OnPropertyChanged();
            }
        }

        private DetectionType luminosity;
        public DetectionType Luminosity
        {
            get { return luminosity; }
            set
            {
                luminosity = value;
                OnPropertyChanged();
            }
        }

        private DetectionType motion;
        public DetectionType Motion
        {
            get { return motion; }
            set
            {
                motion = value;
                OnPropertyChanged();
            }
        }

        private DoorStatus door;
        public DoorStatus Door
        {
            get { return door; }
            set
            {
                door = value;
                OnPropertyChanged();
            }
        }

        private bool doorLock;
        public bool DoorLock
        {
            get { return doorLock; }
            set { 
                    doorLock = value;
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
