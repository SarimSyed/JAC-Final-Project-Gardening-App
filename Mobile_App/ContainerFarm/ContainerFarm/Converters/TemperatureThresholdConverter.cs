using ContainerFarm.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Converters
{
    /// <summary>
    /// The temperature threshold text color converter.
    /// </summary>
    public class TemperatureThresholdConverter : IValueConverter    
    {
        /// <summary>
        /// Converts the value to a color.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double threshold;

            // Convert the threshold value
            try
            {
                threshold = System.Convert.ToDouble(value);
            }
            catch (Exception)
            {
                threshold = (double) value;
            }

            Color color = Colors.Black;

            // Set the low warning color
            if (threshold < App.Settings.TemperatureLowThreshold)
                color = ThresholdValueColors.TEMPERATURE_LOW;
            // Set the high warning color
            else if (threshold > App.Settings.TemperatureHighThreshold)
                color = ThresholdValueColors.TEMPERATURE_HIGH;

            return color;
        }

        /// <summary>
        /// Converts the color back to null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = value as Color;
            return color.ToHex();
        }
    }
}
