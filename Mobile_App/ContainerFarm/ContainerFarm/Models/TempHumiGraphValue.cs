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
                return $"{EnqueuedTime.Month:M} {EnqueuedTime.Day}, {EnqueuedTime.Year}  {EnqueuedTime.Hour - UTC_EST_TIME_HOUR_DIFF}:{EnqueuedTime.Minute} {AM_PM_VALUE()}";
            }
        }

        private string AM_PM_VALUE()
        {
            return EnqueuedTime.Hour >= 0 && EnqueuedTime.Hour <= 12
                ? "AM"
                : "PM";
        }
    }
}
