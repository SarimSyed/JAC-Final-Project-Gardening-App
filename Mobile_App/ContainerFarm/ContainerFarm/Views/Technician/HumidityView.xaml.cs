using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using ContainerFarm.Repos;

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

    public ISeries[] Series { get; set; } =
    {
        new LineSeries<double>
        {
            Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },

        }
    };
}