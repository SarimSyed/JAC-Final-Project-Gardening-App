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
                return $"{EnqueuedTime.ToString("M")}, {EnqueuedTime.Year}  {GetHourFrom12HourClock()}:{EnqueuedTime.Minute} {AM_PM_VALUE()}";
            }
        }

        /// <summary>
        /// Gets the AM or PM string representation of the Hour.
        /// </summary>
        /// <returns>The AM or PM string representation of the Hour.</returns>
        private string AM_PM_VALUE()
        {
            return EnqueuedTime.Hour >= 0 && EnqueuedTime.Hour <= 12
                ? "AM"
                : "PM";
        }

        private int GetHourFrom12HourClock()
        {
            int UTC_EST_difference = EnqueuedTime.Hour - UTC_EST_TIME_HOUR_DIFF;

            return UTC_EST_difference > 12
                ? UTC_EST_difference - 12
                : UTC_EST_difference;
        }
    }
}
