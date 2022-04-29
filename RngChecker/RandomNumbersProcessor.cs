using MathNet.Numerics.Statistics;
using System.Diagnostics;

namespace RandomChecker;

internal class RandomNumbersProcessor
{
    private const string digits = "E3";
    private readonly double inputAverage;
    private readonly double inputDeviation;
    private readonly Stopwatch sw = new();
    private readonly List<double> numbers = new();

    internal RandomNumbersProcessor(double inputAverage, double inputDeviation)
    {
        this.inputAverage = inputAverage;
        this.inputDeviation = inputDeviation;
    }

    internal void StartTimer()
    {
        if (this.sw.IsRunning)
        {
            this.sw.Stop();
            this.sw.Reset();
        }

        this.sw.Start();
    }

    internal void Generate(int dataCount)
    {
        using var rng = new Rng();
        this.numbers.AddRange(rng.GetNormRandoms(this.inputAverage, this.inputDeviation, dataCount));
    }

    internal void StopTimer() => this.sw.Stop();

    // 尖度は0を中心とする流儀と3を中心とする流儀があるが、MathNetは0の方
    internal void MakeResult()
    {
        const double normThreshold = 1.0e-3;
        const double statThreshold = 1.5;
        var stat = new DescriptiveStatistics(this.numbers);
        var average = stat.Mean;
        var deviation = stat.StandardDeviation;
        var skewness = stat.Skewness;
        var kurtosis = stat.Kurtosis;

        Debug.WriteLine($"Process time: {this.sw.Elapsed:s\\.fff} sec");
        Debug.WriteLine($"Average: {average.ToString(digits)}");
        Debug.WriteLine($"Standard deviation: {deviation.ToString(digits)}");
        Debug.WriteLine($"Skewness: {skewness.ToString(digits)}");
        Debug.WriteLine($"Kurtosis: {kurtosis.ToString(digits)}");
        Debug.WriteLine(string.Empty);
        Debug.WriteLine($"Average check: {(Math.Abs(average - this.inputAverage) / this.inputAverage) < (this.inputAverage * normThreshold)}");
        Debug.WriteLine($"Standard deviation check: {(Math.Abs(deviation - this.inputDeviation) / this.inputDeviation) < (this.inputDeviation * normThreshold)}");
        Debug.WriteLine($"Skewness check: {Math.Abs(skewness) < statThreshold}");
        Debug.WriteLine($"Kurtosis check: {Math.Abs(kurtosis) < statThreshold}");
    }
}
