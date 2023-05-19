using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using ContainerFarm.Repos;
using ContainerFarm.Models;
using System.Collections.ObjectModel;

namespace ContainerFarm.Views.Technician;

public partial class HumidityView : ContentPage
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III 
    /// <summary>
    /// THis class is used to inizialize a chart for the humidity sensor over the last x days.
    /// </summary>
    public HumidityView()
	{
		InitializeComponent();

        pie_chart.Series = Series;

        humidity_cv.ItemsSource = ContainerRepo.HumidityValues.OrderByDescending(humi => humi.EnqueuedTime); 

    }

    /// <summary>
    /// The Series for the humidity values chart.
    /// </summary>
    public ISeries[] Series { get; set; } =
    {
        new LineSeries<double>
        {
            Values = ContainerRepo.HumidityValues.Select(humiValue => humiValue.Value),
            DataLabelsFormatter = (point) => $"Humidity: {point.PrimaryValue.ToString("F2")}",
            TooltipLabelFormatter = (point) => $"Humidity: {point.PrimaryValue.ToString("F2")}"
        }
    };
}