using OxyPlot;
using OxyPlot.Axes;

namespace Volkin.TRS.SpectralAnalysis
{
    internal static class Extensions
    {
        public static void AddAxes(this PlotModel plotModel, string xTitle, string yTitle)
        {
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = xTitle,
                FontSize = 14,
                TitleFontSize = 14,
                FontWeight = FontWeights.Bold,
                TitleFontWeight = FontWeights.Bold
            });

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = yTitle,
                FontSize = 14,
                TitleFontSize = 14,
                FontWeight = FontWeights.Bold,
                TitleFontWeight = FontWeights.Bold
            });
        }
    }
}
