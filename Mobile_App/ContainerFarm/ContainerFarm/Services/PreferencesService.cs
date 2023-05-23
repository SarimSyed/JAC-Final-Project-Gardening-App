using ContainerFarm.Enums;
using ContainerFarm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Services
{
    /// <summary>
    /// Service for setting and updating app preferences.
    /// </summary>
    public static class PreferencesService
    {
        /// <summary>
        /// Sets the app settings values from the app preferences.
        /// </summary>
        public static void SetValuesFromPreferences()
        {
            try
            {
                Preferences.Default.Clear();
                // Set the TemperatureHighThreshold
                if (Preferences.Default.ContainsKey(ThresholdKeys.TEMPERATURE_HIGH))
                {
                    App.Settings.TemperatureHighThreshold = Preferences.Default.Get(ThresholdKeys.TEMPERATURE_HIGH, App.Settings.TemperatureHighThreshold);
                }

                // Set the TemperatureLowThreshold
                if (Preferences.Default.ContainsKey(ThresholdKeys.TEMPERATURE_LOW))
                {
                    App.Settings.TemperatureLowThreshold = Preferences.Default.Get(ThresholdKeys.TEMPERATURE_LOW, App.Settings.TemperatureLowThreshold);
                }

                // Set the HumidityHighThreshold
                if (Preferences.Default.ContainsKey(ThresholdKeys.HUMIDITY_HIGH))
                {
                    App.Settings.HumidityHighThreshold = Preferences.Default.Get(ThresholdKeys.HUMIDITY_HIGH, App.Settings.HumidityHighThreshold);
                }

                // Set the HumidityLowThreshold
                if (Preferences.Default.ContainsKey(ThresholdKeys.HUMIDITY_LOW))
                {
                    App.Settings.HumidityLowThreshold = Preferences.Default.Get(ThresholdKeys.HUMIDITY_LOW, App.Settings.HumidityLowThreshold);
                }

                // Set the WaterLevelHighThreshold
                if (Preferences.Default.ContainsKey(ThresholdKeys.WATER_LEVEL_HIGH))
                {
                    App.Settings.WaterLevelHighThreshold = Preferences.Default.Get(ThresholdKeys.WATER_LEVEL_HIGH, App.Settings.WaterLevelHighThreshold);
                }

                // Set the WaterLevelLowThreshold
                if (Preferences.Default.ContainsKey(ThresholdKeys.WATER_LEVEL_LOW))
                {
                    App.Settings.WaterLevelLowThreshold = Preferences.Default.Get(ThresholdKeys.WATER_LEVEL_LOW, App.Settings.WaterLevelLowThreshold);
                }

                // Set the TelemetryInterval
                if (Preferences.Default.ContainsKey(TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY))
                {
                    TelemetryIntervalModel.TelemetryInterval = Preferences.Default.Get(TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY, TelemetryIntervalModel.TelemetryInterval);
                }
            }
            catch (NotSupportedException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void UpdateSpecificPreference(string preferenceKey, int preferenceValue)
        {
            if (Preferences.Default.ContainsKey(preferenceKey))
                Preferences.Default.Set(preferenceKey, preferenceValue);
        }
        public static void UpdateSpecificPreference(string preferenceKey, string preferenceValue)
        {
            if (Preferences.Default.ContainsKey(preferenceKey))
                Preferences.Default.Set(preferenceKey, preferenceValue);
        }
        public static void UpdateSpecificPreference(string preferenceKey, double preferenceValue)
        {
            if (Preferences.Default.ContainsKey(preferenceKey))
                Preferences.Default.Set(preferenceKey, preferenceValue);
        }
    }
}
