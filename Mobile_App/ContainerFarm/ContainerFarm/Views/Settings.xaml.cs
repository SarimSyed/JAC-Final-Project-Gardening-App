using ContainerFarm.Enums;
using ContainerFarm.Models;
using ContainerFarm.Services;

namespace ContainerFarm.Views;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();

		this.BindingContext = App.Settings;
        telemetryInterval_pc.BindingContext = TelemetryIntervalModel.TelemetryInterval;

        SetPickerAndIndex(SettingsPickerRanges.TEMPERATURE_HIGH_START, SettingsPickerRanges.TEMPERATURE_HIGH_COUNT, temperatureHigh_pc, Preferences.Default.Get(ThresholdKeys.TEMPERATURE_HIGH, App.Settings.TemperatureHighThreshold));
        SetPickerAndIndex(SettingsPickerRanges.TEMPERATURE_LOW_START, SettingsPickerRanges.TEMPERATURE_LOW_COUNT, temperatureLow_pc, Preferences.Default.Get(ThresholdKeys.TEMPERATURE_LOW, App.Settings.TemperatureLowThreshold));

        SetPickerAndIndex(SettingsPickerRanges.HUMIDITY_HIGH_START, SettingsPickerRanges.HUMIDITY_HIGH_COUNT, humidityHigh_pc, Preferences.Default.Get(ThresholdKeys.HUMIDITY_HIGH, App.Settings.HumidityHighThreshold));
        SetPickerAndIndex(SettingsPickerRanges.HUMIDITY_LOW_START, SettingsPickerRanges.HUMIDITY_LOW_COUNT, humidityLow_pc, Preferences.Default.Get(ThresholdKeys.HUMIDITY_LOW, App.Settings.HumidityLowThreshold));

        SetPickerAndIndex(SettingsPickerRanges.WATER_LEVEL_HIGH_START, SettingsPickerRanges.WATER_LEVEL_HIGH_COUNT, waterLevelHigh_pc, Preferences.Default.Get(ThresholdKeys.WATER_LEVEL_HIGH, App.Settings.WaterLevelHighThreshold));
        SetPickerAndIndex(SettingsPickerRanges.WATER_LEVEL_LOW_START, SettingsPickerRanges.WATER_LEVEL_LOW_COUNT, waterLevelLow_pc, Preferences.Default.Get(ThresholdKeys.WATER_LEVEL_LOW, App.Settings.WaterLevelLowThreshold));

        SetPickerAndIndex(SettingsPickerRanges.TELEMETRY_INTERVAL_START, SettingsPickerRanges.TELEMETRY_INTERVAL_COUNT, telemetryInterval_pc, Preferences.Default.Get(TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY, TelemetryIntervalModel.TelemetryInterval));
    }

    /// <summary>
    /// Sets the specified picker values.
    /// </summary>
    /// <param name="start">The starting value of the picker.</param>
    /// <param name="count">The count after the starting value (ex: starting value: 1 count: 20, value range: 1-20).</param>
    /// <param name="picker">The specified picker in the settings page.</param>
    /// <param name="threshold">The threshold value.</param>
    private void SetPickerAndIndex(int start, int count, Picker picker, double threshold)
    {
        List<int> pickerValues = Enumerable.Range(start, count).ToList();
        picker.ItemsSource = pickerValues;

        int index = pickerValues.FindIndex(t => t == threshold);
        picker.SelectedIndex = index;
    }

    /// <summary>
    /// Updates the specific preferences from the settings app.
    /// </summary>
    private void UpdatePreferences()
    {
        try
        {
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.TEMPERATURE_HIGH, Convert.ToDouble(temperatureHigh_pc.SelectedItem));
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.TEMPERATURE_LOW, Convert.ToDouble(temperatureLow_pc.SelectedItem));
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.HUMIDITY_HIGH, Convert.ToDouble(humidityHigh_pc.SelectedItem));
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.HUMIDITY_LOW, Convert.ToDouble(humidityLow_pc.SelectedItem));
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.WATER_LEVEL_HIGH, Convert.ToDouble(waterLevelHigh_pc.SelectedItem));
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.WATER_LEVEL_LOW, Convert.ToDouble(waterLevelLow_pc.SelectedItem));
            PreferencesService.UpdateSpecificPreference(TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY, Convert.ToDouble(telemetryInterval_pc.SelectedItem));
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

    protected override bool OnBackButtonPressed()
    {
        UpdatePreferences();

        return base.OnBackButtonPressed();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        UpdatePreferences();
    }

    /// <summary>
    /// Updates the preferences of the specified picker.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Settings_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;

        if (picker == null || picker.SelectedItem == null || picker.SelectedIndex == -1) return;

        UpdatePreferences();
    }

    /// <summary>
    /// Updates the telemetry interval when the selected index is changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void telemetryInterval_pc_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;

        if (picker == null || picker.SelectedItem == null || picker.SelectedIndex == -1) return;

        TelemetryIntervalModel.SetTelemetryInterval((int)picker.SelectedItem);
        
        if (picker.IsFocused)
            TelemetryIntervalModel.IsChanged = true;

        Settings_Picker_SelectedIndexChanged(sender, e);
    }
}