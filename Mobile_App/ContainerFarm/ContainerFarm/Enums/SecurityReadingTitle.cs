﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Enums
{
    /// <summary>
    /// Holds the sensor and actuator names of the Security subsystem.
    /// </summary>
    public static class SecurityReadingTitle
    {
        public const string DOOR = "door";
        public const string DOOR_OPEN = "open";
        public const string DOOR_CLOSE = "closed";
        public const string MOTION = "motion";
        public const string MOTION_DETECTED = "detected";
        public const string NOISE = "noise";
        public const string LUMINOSITY = "luminosity";

        public const byte LUMINOSITY_THRESHOLD = 30;
        public const byte NOISE_LOW_THRESHOLD = 100;
        public const byte NOISE_HIGH_THRESHOLD = 180;

        public const byte TURN_ON = 1;
        public const byte TURN_OFF = 0;
    }
}
