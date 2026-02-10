using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

/// <summary>
/// Runs benchmarks.
/// </summary>
public sealed class Runner : MonoBehaviour
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public bool IsRunning => !enabled; // We disable component during benchmark run.



    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    [SerializeField] private KeyCode m_BenchmarkKey = KeyCode.R;




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Unity Callbacks
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private void Awake() => Application.targetFrameRate = 30;
    public void Update()
    {
        if (Input.GetKeyDown(m_BenchmarkKey))
        {
            Benchmark();
        }
    }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public void Benchmark()
    {
        if (IsRunning) return;
        if (CacheScrambler.State != CacheScrambler.ScramblerState.Initialized) return;
        enabled = false;
        try
        {
            // Runs benchmarks themselves.
            EclipseRunner.Instance.Initialize();
            NaninovelRunner.Instance.Initialize();
            RimWorldRunner.Instance.Initialize();
            NativeRunner.Instance.Initialize();
            IdleRunner.Instance.Initialize();

            const int Repeats = 25;
            CacheScrambler.ScrambleMemory();
            BenchmarkResults eclipse = EclipseRunner.Instance.ExecuteBenchmark(Repeats);

            CacheScrambler.ScrambleMemory();
            BenchmarkResults naninovel = NaninovelRunner.Instance.ExecuteBenchmark(Repeats);

            CacheScrambler.ScrambleMemory();
            BenchmarkResults rimworld = RimWorldRunner.Instance.ExecuteBenchmark(Repeats);

            CacheScrambler.ScrambleMemory();
            BenchmarkResults native = NativeRunner.Instance.ExecuteBenchmark(Repeats);

            CacheScrambler.ScrambleMemory();
            BenchmarkResults idle = IdleRunner.Instance.ExecuteBenchmark(Repeats);

            const string BenchmarkHeader = "Benchmark";
            const string BestAverage = "Best (Avr.)(μs)";
            const string WorstAverage = "Worst (Avr.)(μs)";
            const string ComplexityHeader = "Complexity";
            const int RunnerAmount = 5;
            const int TableHeight = RunnerAmount + 2;
            string[] names = new string[TableHeight] {
                BenchmarkHeader,
                string.Empty,
                EclipseRunner.Instance.Name,
                NaninovelRunner.Instance.Name,
                RimWorldRunner.Instance.Name,
                NativeRunner.Instance.Name,
                IdleRunner.Instance.Name,
            };

            string[] bests = new string[TableHeight] {
                BestAverage,
                string.Empty,
                eclipse.BestAverageMilliseconds.ToString("0.0000"),
                naninovel.BestAverageMilliseconds.ToString("0.0000"),
                rimworld.BestAverageMilliseconds.ToString("0.0000"),
                native.BestAverageMilliseconds.ToString("0.0000"),
                idle.BestAverageMilliseconds.ToString("0.0000"),
            };

            string[] worsts = new string[TableHeight] {
                WorstAverage,
                string.Empty,
                eclipse.WorstAverageMilliseconds.ToString("0.0000"),
                naninovel.WorstAverageMilliseconds.ToString("0.0000"),
                rimworld.WorstAverageMilliseconds.ToString("0.0000"),
                native.WorstAverageMilliseconds.ToString("0.0000"),
                idle.WorstAverageMilliseconds.ToString("0.0000"),
            };

            string[] complexities = new string[TableHeight] {
                ComplexityHeader,
                string.Empty,
                EclipseRunner.Instance.TimeComplexity,
                NaninovelRunner.Instance.TimeComplexity,
                RimWorldRunner.Instance.TimeComplexity,
                NativeRunner.Instance.TimeComplexity,
                IdleRunner.Instance.TimeComplexity,
            };

            Span<int> columns = stackalloc int[4] {
                names.Max(n => n.Length),
                bests.Max(n => n.Length),
                worsts.Max(n => n.Length),
                complexities.Max(c => c.Length),
            };

            // Behold hard-coded mayhem!
            // It's just a testing code anyway.
            StringBuilder builder = new();
            names[1] = builder.Append('-', columns[0]).ToString();
            builder.Clear();
            bests[1] = builder.Append('-', columns[1]).ToString();
            builder.Clear();
            worsts[1] = builder.Append('-', columns[2]).ToString();
            builder.Clear();
            complexities[1] = builder.Append('-', columns[3]).ToString();
            builder.Clear();

            for (int row = 0; row < TableHeight; row++)
            {
                WriteBlock(builder, names[row], columns[0]);
                Separator(builder);
                WriteBlock(builder, bests[row], columns[1]);
                Separator(builder);
                WriteBlock(builder, worsts[row], columns[2]);
                Separator(builder);
                WriteBlock(builder, complexities[row], columns[3]);
                builder.AppendLine();
            }

            /*
Benchmark concluded:
Benchmark                       | Best (Avr.)(μs) | Worst (Avr.)(μs) | Complexity
------------------------------- | --------------- | ---------------- | ----------
Eclipse IService.Instance       | 0.0117          | 0.0103           | O(1)      
Naninovel Engine.GetService<>() | 0.0345          | 0.0345           | O(1)      
RimWorld Game.GetComponent<>()  | 0.0167          | 0.4001           | O(n) n:7  
Native GetField                 | 0.0104          | 0.0101           | O(1)      
Idle (Control)                  | 0.0003          | 0.0003           | O(1)        
            */

            Debug.Log($"Benchmark concluded:\n{builder}");

            // Simplifications:
            static void WriteBlock(StringBuilder builder, string content, int max)
            {
                max = Math.Max(max, content.Length);
                builder.Append(content);
                builder.Append(' ', max - content.Length);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void Separator(StringBuilder builder) => builder.Append(" | ");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        enabled = true;
    }
}
