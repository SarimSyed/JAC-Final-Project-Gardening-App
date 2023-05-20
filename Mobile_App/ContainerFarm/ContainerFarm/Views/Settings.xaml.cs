using ContainerFarm.Enums;
using ContainerFarm.Models;

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

        //picker.BindingContext = threshold;
    }

    private void SetPreferences()
    {
        try
        {
            Preferences.Default.Set(ThresholdKeys.TEMPERATURE_HIGH, (int) temperatureHigh_pc.SelectedItem);
            Preferences.Default.Set(ThresholdKeys.TEMPERATURE_LOW, (int)temperatureLow_pc.SelectedItem);
            Preferences.Default.Set(ThresholdKeys.HUMIDITY_HIGH, (int)humidityHigh_pc.SelectedItem);
            Preferences.Default.Set(ThresholdKeys.HUMIDITY_LOW, (int)humidityLow_pc.SelectedItem);
            Preferences.Default.Set(ThresholdKeys.WATER_LEVEL_HIGH, (int)waterLevelHigh_pc.SelectedItem);
            Preferences.Default.Set(ThresholdKeys.WATER_LEVEL_LOW, (int)waterLevelLow_pc.SelectedItem);
            Preferences.Default.Set(TelemetryIntervalModel.TELEMETRY_INTERVAL_PROPERTY, (int)telemetryInterval_pc.SelectedItem);
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
        SetPreferences();

        return base.OnBackButtonPressed();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        SetPreferences();
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;

        if (picker == null || picker.SelectedItem == null || picker.SelectedIndex == -1) return;

        SetPreferences();
    }

    private void telemetryInterval_pc_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;
        TelemetryIntervalModel.SetTelemetryInterval((int)picker.SelectedItem);

        Picker_SelectedIndexChanged(sender, e);
    }
}