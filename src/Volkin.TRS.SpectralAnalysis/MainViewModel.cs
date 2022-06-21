using System;
using OxyPlot;
using OxyPlot.Series;
using static System.Math;

namespace Volkin.TRS.SpectralAnalysis
{
    public class MainViewModel
    {
        // X0:T:Dx -- Вектор времени сигнала в интервале 0...1.499 с
        private const double X0 = 0;
        private const double T = 1 / Fs; // Период дискретизации(шаг отсчетов сигнала по времени)
        private const double X1 = L - T;

        private const double Fs = 2000; // Частота дискретизации (частота отсчётов) исходного сигнала
        private const double L = 1.5; // Длительность сигнала, с

        private const double A1 = 0.7; // Амплитуда первой составляющей сигнала
        private const double A2 = 1; // Амплитуда второй составляющей сигнала
        private const double A3 = 2; // Амплитуда третьей составляющей сигнала
        private const double Om1 = 120; // Частота первой оставляющей
        private const double Om2 = 80; // Частота второй оставляющей
        private const double Om3 = 70; // Частота третьей оставляющей

        public MainViewModel()
        {
            SignalModel = new PlotModel { Title = "Сигнал, формируемый РСПИ" };
            SignalModel.AddAxes("t, c", "s(t)");
            SignalModel.Series.Add(new FunctionSeries(SignalFunc, X0, X1, T));

            SpectrumModel = new PlotModel { Title = "Распределение по частотам амплитуд составляющих" };
            SpectrumModel.AddAxes("f, Гц", "|S(f)|");

            SignalWithNoiseModel = new PlotModel { Title = "Смесь сигнала и шума в канале" };
            SignalWithNoiseModel.AddAxes("t, c", "n(t)");
            SignalWithNoiseModel.Series.Add(new FunctionSeries(SignalWithNoiseFunc, X0, X1, T));

            SignalWithNoiseSpectrumModel = new PlotModel { Title = "Амплитудный спектр смеси сигнала с шумом" };
            SignalWithNoiseSpectrumModel.AddAxes("f, Гц", "|S(f)|");
        }

        public PlotModel SignalModel { get; }
        public PlotModel SpectrumModel { get; }
        public PlotModel SignalWithNoiseModel { get; }
        public PlotModel SignalWithNoiseSpectrumModel { get; }

        /// <summary> Функция шума </summary>
        private static double NoiseFunc() => 0.2 * new Random().NextDouble();

        /// <summary> Первая составляющая сигнала </summary>
        private double S1Func(double x) => A1 * Sin(2 * PI * Om1 * x);

        /// <summary> Вторая составляющая сигнала </summary>
        private double S2Func(double x) => A2 * Sin(2 * PI * Om2 * x);

        /// <summary> Вторая составляющая сигнала </summary>
        private double S3Func(double x) => A3 * Sin(2 * PI * Om3 * x);

        /// <summary> Сигнал, формируемый радиоэлектронной системой передачи </summary>
        private double SignalFunc(double x) => S1Func(x) + S2Func(x) + S3Func(x);

        /// <summary> Сигнал, формируемый радиоэлектронной системой передачи </summary>
        private double SignalWithNoiseFunc(double x) => NoiseFunc() + SignalFunc(x);

        //TODO: <summary> Вычисление спектральной плотности сигнала (БПФ) </summary>
        //private double SpectrumFunc(double x) => AForge.Math.FourierTransform.FFT(new[] { new Complex(x, 0) }, direction: FourierTransform.Direction.Forward)

    }
}