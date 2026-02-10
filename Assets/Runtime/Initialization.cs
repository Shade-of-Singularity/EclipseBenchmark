using Naninovel;
using UnityEngine;

public static class Initialization
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static async void Initialize()
    {
        Eclipse.EclipseLogger.Initialize(Debug.Log, Debug.LogWarning, Debug.LogError, Debug.LogException);
        await RuntimeInitializer.Initialize();
        await Eclipse.Engine.Initialize();
        await CacheScrambler.Initialize();
        Debug.Log("Benchmark initialization completed!");
    }
}
