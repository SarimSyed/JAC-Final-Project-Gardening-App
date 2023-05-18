using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using ContainerFarm.Repos;

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

        temperature_cv.ItemsSource = ContainerRepo.TemperatureValues;
    }

    public ISeries[] Series { get; set; } =
    {
        new LineSeries<double>
        {
            Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
            
        }
    };
}