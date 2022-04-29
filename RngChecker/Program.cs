using MathNet.Numerics.Statistics;
using RandomChecker;

const double inputAverage = 5;
const double inputDeviation = 1;
const int dataCount = 10000000;
var rngProcessor = new RandomNumbersProcessor(inputAverage, inputDeviation);
rngProcessor.StartTimer();
rngProcessor.Generate(dataCount);
rngProcessor.StopTimer();
rngProcessor.MakeResult();
