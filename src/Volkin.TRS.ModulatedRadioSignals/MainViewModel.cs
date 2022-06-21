using System;
using System.Globalization;
using OxyPlot;
using OxyPlot.Series;

namespace Volkin.TRS.ModulatedRadioSignals
{
    public class MainViewModel
    {
        private const double X0 = 0;
        private const double X1 = 1;
        private const double Dx = 0.001;

        private const double OmAm = 10; // Частота модулирующего колебания при АМ, [рад / с]
        private const double V = 1; // Амплитуда несущего колебания, [В]
        private const double F = 100; // Частота несущего колебания, [Гц]
        private const double K = 9; // Коэффициент пропорциональности

        /// <summary> Частота модулирующего колебания при ЧМ, [рад / с] </summary>
        private string _omFm = "0.2";

        /// <summary> Коэффициент АМ </summary>
        private string _m = "0.8";

        /// <summary> Частота модулирующего колебания при ФМ, [Гц] </summary>
        private string _fPm = "15";

        public MainViewModel()
        {
            FmModel = new PlotModel { Title = "ЧМ" };
            FmModel.Series.Add(new FunctionSeries(FmFunc, X0, X1, Dx));

            FmWithNoiseModel = new PlotModel { Title = "ЧМ с шумом" };
            FmWithNoiseModel.Series.Add(new FunctionSeries(NoiseFmFunc, X0, X1, Dx));


            PmModel = new PlotModel { Title = "ФМ" };
            PmModel.Series.Add(new FunctionSeries(PmFunc, X0, X1, Dx));

            PmWithNoiseModel = new PlotModel { Title = "ФМ с шумом" };
            PmWithNoiseModel.Series.Add(new FunctionSeries(NoisePmFunc, X0, X1, Dx));


            AmModel = new PlotModel { Title = "АМ" };
            AmModel.Series.Add(new FunctionSeries(AmFunc, X0, X1, Dx));

            AmWithNoiseModel = new PlotModel { Title = "АМ с шумом" };
            AmWithNoiseModel.Series.Add(new FunctionSeries(NoiseAmFunc, X0, X1, Dx));
        }

        /// <summary> Частота модулирующего колебания при ЧМ, [рад / с] </summary>
        public string OmFm
        {
            get => _omFm;
            set
            {
                _omFm = value;

                if (!Double.TryParse(value, out double _))
                    return;//TODO: Ошибка

                RedrawPlot(FmModel, FmFunc);
                RedrawPlot(FmWithNoiseModel, NoiseFmFunc);
            }
        }

        /// <summary> Частота модулирующего колебания при ФМ, [Гц] </summary>
        public string Fpm
        {
            get => _fPm;
            set
            {
                _fPm = value;

                if (!Double.TryParse(value, out double _))
                    return;//TODO: Ошибка

                RedrawPlot(PmModel, PmFunc);
                RedrawPlot(PmWithNoiseModel, NoisePmFunc);
            }
        }

        /// <summary> Коэффициент АМ </summary>
        public string M
        {
            get => _m;
            set
            {
                _m = value;

                if (!Double.TryParse(value, out double _))
                    return;//TODO: Ошибка

                RedrawPlot(AmModel, AmFunc);
                RedrawPlot(AmWithNoiseModel, NoiseAmFunc);
            }
        }

        public PlotModel FmModel { get; }
        public PlotModel FmWithNoiseModel { get; }

        public PlotModel PmModel { get; }
        public PlotModel PmWithNoiseModel { get; }

        public PlotModel AmModel { get; }
        public PlotModel AmWithNoiseModel { get; }

        private static void RedrawPlot(PlotModel plotModel, Func<double, double> func)
        {
            plotModel.Series.Clear();
            plotModel.Series.Add(new FunctionSeries(func, X0, X1, Dx));
            plotModel.InvalidatePlot(true);
        }

        private static double GetDoubleFromString(string str)
            => Double.Parse(str.Replace(',', '.'), CultureInfo.InvariantCulture);

        /// <summary> Функция шума </summary>
        private static double NoiseFunc() => (V / 2) * new Random().NextDouble();

        /// <summary> Закон частотной модуляции </summary>
        private double FmFunc(double x) => 1 * Math.Cos(2 * Math.PI * F * K * 0.2 * Math.Sin(GetDoubleFromString(_omFm) * x) * x);

        /// <summary> Закон фазовой модуляции </summary>
        private double PmFunc(double x) => V * Math.Cos(2 * Math.PI * F * x + K * Math.Cos(2 * Math.PI * GetDoubleFromString(_fPm) * x));

        /// <summary> Закон амплитудной модуляции </summary>
        private double AmFunc(double x) => V * (1 + GetDoubleFromString(_m) * Math.Cos(OmAm * x)) * Math.Cos(2 * Math.PI * F * x);


        /// <summary> Закон частотной модуляции с шумом </summary>
        private double NoiseFmFunc(double x) => NoiseFunc() + FmFunc(x);

        /// <summary> Закон фазовой модуляции с шумом </summary>
        private double NoisePmFunc(double x) => NoiseFunc() + PmFunc(x);

        /// <summary> Закон амплитудной модуляции с шумом </summary>
        private double NoiseAmFunc(double x) => NoiseFunc() + AmFunc(x);
    }
}