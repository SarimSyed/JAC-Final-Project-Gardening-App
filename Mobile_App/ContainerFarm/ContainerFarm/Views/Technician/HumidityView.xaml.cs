using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;

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
    }

    public ISeries[] Series { get; set; } =
    {
        new LineSeries<double>
        {
            Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },

        }
    };
}