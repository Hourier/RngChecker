using System.Security.Cryptography;

namespace RandomChecker;

internal class Rng : IDisposable
{
    private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

    internal Rng()
    {
    }

    public void Dispose() => this.rng.Dispose();

    // 指定した平均値及び標準偏差を持つ乱数群を、正規分布に従って指定個数分生成する
    public IEnumerable<double> GetNormRandoms(double average, double deviation, int dataCount)
    {
        foreach (var _ in Enumerable.Range(0, dataCount))
        {
            yield return this.GetOneNormRandom(average, deviation);
        }
    }

    // Box–Muller法により正規分布に従う乱数を1つ生成する
    // 正規分布の性質より、平均値及び標準偏差の近傍に乱数が集中する
    // 今後の利便性を考えてアクセス修飾子はpublicにしておく
    public double GetOneNormRandom(double average, double deviation)
    {
        var random1 = this.GetOneRandom();
        var random2 = this.GetOneRandom();
        var norm = Math.Sqrt(-2 * Math.Log(random1, Math.E)) * Math.Cos(2 * Math.PI * random2);
        return average + deviation * norm;
    }

    // 一様分布に従う乱数を1つ生成する
    private double GetOneRandom()
    {
        var bytes = new byte[sizeof(long)];
        this.rng.GetBytes(bytes);
        var value = BitConverter.ToInt64(bytes, 0);
        return Math.Abs((double)value / long.MaxValue);
    }
}
