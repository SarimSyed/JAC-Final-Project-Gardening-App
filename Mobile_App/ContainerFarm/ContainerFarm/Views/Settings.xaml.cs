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

        SetPickerAndIndex(15, 36, temperatureHigh_pc, Preferences.Default.Get("temperatureHigh", App.Settings.TemperatureHighThreshold));
        SetPickerAndIndex(-20, 36, temperatureLow_pc, Preferences.Default.Get("temperatureLow", App.Settings.TemperatureLowThreshold));

        SetPickerAndIndex(50, 51, humidityHigh_pc, Preferences.Default.Get("humidityHigh", App.Settings.HumidityHighThreshold));
        SetPickerAndIndex(0, 51, humidityLow_pc, Preferences.Default.Get("humidityLow", App.Settings.HumidityLowThreshold));

        SetPickerAndIndex(50, 51, waterLevelHigh_pc, Preferences.Default.Get("waterLevelHigh", App.Settings.WaterLevelHighThreshold));
        SetPickerAndIndex(0, 51, waterLevelLow_pc, Preferences.Default.Get("waterLevelLow", App.Settings.WaterLevelLowThreshold));

        SetPickerAndIndex(1, 20, telemetryInterval_pc, Preferences.Default.Get("telemetryInterval", TelemetryIntervalModel.TelemetryInterval));
    }

    private void SetPickerAndIndex(int start, int count, Picker picker, int threshold)
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
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.TEMPERATURE_HIGH, (int)temperatureHigh_pc.SelectedItem);
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.TEMPERATURE_LOW, (int)temperatureLow_pc.SelectedItem);
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.HUMIDITY_HIGH, (int)humidityHigh_pc.SelectedItem);
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.HUMIDITY_LOW, (int)humidityLow_pc.SelectedItem);
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.WATER_LEVEL_HIGH, (int)waterLevelHigh_pc.SelectedItem);
            PreferencesService.UpdateSpecificPreference(ThresholdKeys.WATER_LEVEL_LOW, (int)waterLevelLow_pc.SelectedItem);
            PreferencesService.UpdateSpecificPreference(TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY, (int)telemetryInterval_pc.SelectedItem);
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

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;

        if (picker == null || picker.SelectedItem == null || picker.SelectedIndex == -1) return;

        UpdatePreferences();
    }

    private void telemetryInterval_pc_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;

        if (picker == null || picker.SelectedItem == null || picker.SelectedIndex == -1) return;

        TelemetryIntervalModel.SetTelemetryInterval((int)picker.SelectedItem);
        
        if (picker.IsFocused)
            TelemetryIntervalModel.IsChanged = true;

        Picker_SelectedIndexChanged(sender, e);
    }
}