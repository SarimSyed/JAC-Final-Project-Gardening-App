using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using ContainerFarm.Repos;
using ContainerFarm.Models;

namespace ContainerFarm.Views.Technician;

public partial class TemperatureView : ContentPage
{
    /// Connected Tractors (Team #5)
    /// Winter 2023 - April 28th
    /// AppDev III 
    /// <summary>
    /// THis class is used to inizialize a chart for the temperature sensor over the last x days.
    /// </summary>
    public TemperatureView()
	{
		InitializeComponent();

        pie_chart.Series = Series;

        temperature_cv.ItemsSource = ContainerRepo.TemperatureValues.OrderByDescending(temp => temp.EnqueuedTime);
    }

    /// <summary>
    /// Changes the chart when navigated to this page.
    /// </summary>
    /// <param name="args"></param>
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        temperature_cv.ItemsSource = ContainerRepo.TemperatureValues.OrderByDescending(temp => temp.EnqueuedTime);
    }

    /// <summary>
    /// The Series for the temperature values chart.
    /// </summary>
    public ISeries[] Series { get; set; } =
    {
        new LineSeries<double>
        {
            Values = ContainerRepo.TemperatureValues.Select(tempValue => tempValue.Value),
            DataLabelsFormatter = (point) => $"Temperature: {point.PrimaryValue.ToString("F2")}",
            TooltipLabelFormatter = (point) => $"Temperature: {point.PrimaryValue.ToString("F2")}"
        }
    };
}