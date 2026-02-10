using System;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// Implements basic benchmarking functionality for all other runners.
/// </summary>
public abstract partial class AbstractRunner<T> where T : AbstractRunner<T>, new()
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Constants
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected const int OperationsPerInvoke = 5000;




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Static Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static T Instance { get; private set; } = new();




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static double[] Timings = Array.Empty<double>();
    private static readonly Stopwatch Stopwatch = new();
    private static object _consumer;




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public abstract string Name { get; }
    public abstract string TimeComplexity { get; }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public abstract void Initialize();
    public BenchmarkResults ExecuteBenchmark(int repeats)
    {
        if (Timings.Length != repeats) Timings = new double[repeats];

        // Very basic warm-up.
        int warmup = repeats / 3;
        for (int i = 0; i < warmup; i++)
        {
            RunBest();
            RunWorst();
        }

        // Run itself.
        int head = 0;
        for (int i = 0; i < repeats; i++)
        {
            Stopwatch.Restart();
            RunBest();
            Stopwatch.Stop();
            Timings[head++] = (double)Stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
        }
        double best = Timings.Average() / OperationsPerInvoke;

        head = 0;
        for (int i = 0; i < repeats; i++)
        {
            Stopwatch.Restart();
            RunWorst();
            Stopwatch.Stop();
            Timings[head++] = (double)Stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
        }
        double worst = Timings.Average() / OperationsPerInvoke;

        return new(best, worst);
    }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Protected Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected abstract void RunBest();
    protected abstract void RunWorst();




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected static void Consume(object obj) => _consumer = obj;
}
