using ContainerFarm.Enums;
using ContainerFarm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Services
{
    public static class PreferencesService
    {
        public static void SetDefaultPreferences()
        {
            try
            {
                if (Preferences.Default.ContainsKey(ThresholdKeys.TEMPERATURE_HIGH))
                {
                    App.Settings.TemperatureHighThreshold = Preferences.Default.Get(ThresholdKeys.TEMPERATURE_HIGH, App.Settings.TemperatureHighThreshold);
                }
                if (Preferences.Default.ContainsKey(ThresholdKeys.TEMPERATURE_LOW))
                {
                    App.Settings.TemperatureLowThreshold = Preferences.Default.Get(ThresholdKeys.TEMPERATURE_LOW, App.Settings.TemperatureLowThreshold);
                }
                if (Preferences.Default.ContainsKey(ThresholdKeys.HUMIDITY_HIGH))
                {
                    App.Settings.HumidityHighThreshold = Preferences.Default.Get(ThresholdKeys.HUMIDITY_HIGH, App.Settings.HumidityHighThreshold);
                }
                if (Preferences.Default.ContainsKey(ThresholdKeys.HUMIDITY_LOW))
                {
                    App.Settings.HumidityLowThreshold = Preferences.Default.Get(ThresholdKeys.HUMIDITY_LOW, App.Settings.HumidityLowThreshold);
                }
                if (Preferences.Default.ContainsKey(ThresholdKeys.WATER_LEVEL_HIGH))
                {
                    App.Settings.WaterLevelHighThreshold = Preferences.Default.Get(ThresholdKeys.WATER_LEVEL_HIGH, App.Settings.WaterLevelHighThreshold);
                }
                if (Preferences.Default.ContainsKey(ThresholdKeys.WATER_LEVEL_LOW))
                {
                    App.Settings.WaterLevelLowThreshold = Preferences.Default.Get(ThresholdKeys.WATER_LEVEL_LOW, App.Settings.WaterLevelLowThreshold);
                }
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
    }
}
