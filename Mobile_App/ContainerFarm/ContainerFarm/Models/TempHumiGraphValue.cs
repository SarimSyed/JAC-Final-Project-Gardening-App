using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Models
{
    public class TempHumiGraphValue
    {
        private readonly byte UTC_EST_TIME_HOUR_DIFF = 4;

        public string Unit { get; set; }
        public double Value { get; set; }
        public DateTimeOffset EnqueuedTime { get; set; }
        public string EnqueuedTimeFormatted
        {
            get
            {
                int hour = GetHourFrom12HourClock();
                return $"{EnqueuedTime.ToString("M")}, {EnqueuedTime.Year}  {hour}:{EnqueuedTime.Minute} {AM_PM_VALUE(hour)}";
            }
        }

        /// <summary>
        /// Gets the AM or PM string representation of the Hour.
        /// </summary>
        /// <returns>The AM or PM string representation of the Hour.</returns>
        private string AM_PM_VALUE(int hour)
        {
            // Compress this
            int UTC_EST_difference = EnqueuedTime.Hour - UTC_EST_TIME_HOUR_DIFF;

            if (UTC_EST_difference < 0)
                UTC_EST_difference += 24;

            return UTC_EST_difference >= 0 && UTC_EST_difference <= 12
                ? "AM"
                : "PM";
        }

        private int GetHourFrom12HourClock()
        {
            // Compress this
            int UTC_EST_difference = EnqueuedTime.Hour - UTC_EST_TIME_HOUR_DIFF;

            if (UTC_EST_difference < 0)
                UTC_EST_difference += 12;

            return UTC_EST_difference > 12
                ? UTC_EST_difference - 12
                : UTC_EST_difference;
        }
    }
}
