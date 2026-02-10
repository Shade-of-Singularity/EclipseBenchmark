using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// Accesses heap (by a lot) in random order, to make allocate as much as possible in L1, L2, and L3 caches.
/// </summary>
public static partial class CacheScrambler
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                 Constants
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public const string LogPrefix = "[" + nameof(CacheScrambler) + "]";
    public const float MarginIncrease = 2f;
    private const uint KB = 1024;
    private const uint MB = KB * KB;
    private const uint BytesPerItem = 32; // Size of a pointer to an object in bytes.

    // Cache sizes for: 12th Gen Intel(R) Core(TM) i7-12700H
    private const uint L1PerformanceSizeBytes = (6 * 48 * KB) + (6 * 32 * KB);
    private const uint L1EfficientSizeBytes = (8 * 64 * KB) + (8 * 32 * KB);
    private const uint L2PerformanceSizeBytes = (6 * 1280 * KB);
    private const uint L2EfficientSizeBytes = (8 * 2048 * KB);
    public const uint L1SizeBytes = L1PerformanceSizeBytes + L1EfficientSizeBytes;
    public const uint L2SizeBytes = L2PerformanceSizeBytes + L2EfficientSizeBytes;
    public const uint L3SizeBytes = 24 * MB;
    public const uint TotalSizeBytes = L1SizeBytes + L2SizeBytes + L3SizeBytes;




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static ScramblerState State { get; private set; }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static object _consumer;
    private static int StartingLayer;
    private static ulong TotalIndexes;
    private static Quad Root;




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public static async UniTask Initialize()
    {
        switch (State)
        {
            case ScramblerState.Initialized: return;
            case ScramblerState.Initializing: throw new InvalidOperationException($"{LogPrefix} Cannot start initialization twice.");
            default: State = ScramblerState.Initializing; break;
        }

        // Only covers 1st bit in regions of 2 bits: [11][11][11][11]...[11] -> [01][01][01][01]...[01]
        const uint Mask = 0b01010101_01010101_01010101_01010101;
        uint TargetSize = (uint)Math.Ceiling(TotalSizeBytes * MarginIncrease) / BytesPerItem;
        int layers = CountSetBits(TrailBits(TargetSize) & Mask);
        if (layers == 0) throw new Exception("Cache defined zero layers!");

        StartingLayer = layers;
        switch (layers)
        {
            case < 0: throw new NotSupportedException("Cannot have negative layer count.");
            case 0: throw new Exception("Cache defined zero layers!");
            case 1: Root = new(layers); break;
            default:
                // TODO: Fix references and replace Yield() with Delay(1000) instead.
                Quad a1 = new(layers - 1);
                await UniTask.Yield();
                Quad a2 = new(layers - 1);
                await UniTask.Yield();
                Quad b1 = new(layers - 1);
                await UniTask.Yield();
                Quad b2 = new(layers - 1);
                await UniTask.Yield();
                Root = new Quad(a1, a2, b1, b2);
                break;
        }

        const uint QuadSize = 4;
        const int QuadBitAmount = 2; // Bit amount in one region.
        TotalIndexes = QuadSize << ((StartingLayer - 1) * QuadBitAmount); // Essentially '4 ^ layers' in math. E.g. '4 ^ 4 = 256'
        State = ScramblerState.Initialized;
    }

    /// <summary>
    /// Scrambles L1, L2 and L3 memories, to the best of our abilities.
    /// </summary>
    public static void ScrambleMemory()
    {
        switch (State)
        {
            case ScramblerState.Invalid: throw new InvalidOperationException("Cannot call scrambler before initializing it.");
            case ScramblerState.Initializing: throw new InvalidOperationException("Cannot call scrambler while it is not fully initialized.");
            default: break;
        }

        const int QuadBitAmount = 2; // Bit amount in one region.
        for (uint i = 0; i < TotalIndexes; i++)
        {
            Quad quad = Root;
            for (int offset = (StartingLayer - 1) * QuadBitAmount; offset >= QuadBitAmount; offset -= QuadBitAmount)
            {
                quad = ((i >> offset) & 0b11) switch
                {
                    // Starts from the last element, to increase non-linearity.
                    0b00 => (Quad)quad.b2,
                    0b01 => (Quad)quad.b1,
                    0b10 => (Quad)quad.a2,
                    0b11 => (Quad)quad.a1,
                    _ => throw new Exception("How"),
                };
            }

            // Last layer does not contain quads. Instead it contains regular objects.
            Consume((i & 0b11) switch
            {
                // Starts from the last element, to increase non-linearity.
                0b00 => quad.b2, 
                0b01 => quad.b1,
                0b10 => quad.a2,
                0b11 => quad.a1,
                _ => throw new Exception("How"),
            });
        }

        // Simplifications:
        static void Consume(object obj) => _consumer = obj;
    }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    static uint TrailBits(uint input)
    {
        input |= input >> 1;
        input |= input >> 2;
        input |= input >> 4;
        input |= input >> 8;
        input |= input >> 16;
        return input;
    }
    
    static int CountSetBits(uint input)
    {
        // Only supports uint/int:
        //i -= (i >> 1) & 0x55555555;
        //i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
        //return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;

        int result = 0;
        for (int i = 0; i < sizeof(uint) * 8; i += 2)
        {
            result += (int)(input >> i) & 0b1;
        }

        return result;
    }
}
