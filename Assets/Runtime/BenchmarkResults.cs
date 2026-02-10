/// <summary>
/// Container for benchmark results.
/// </summary>
public readonly struct BenchmarkResults
{
    public readonly double BestAverageMilliseconds;
    public readonly double WorstAverageMilliseconds;
    public BenchmarkResults(double averageBest, double averageWorst)
    {
        BestAverageMilliseconds = averageBest;
        WorstAverageMilliseconds = averageWorst;
    }
}