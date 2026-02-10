using Naninovel;
using System.Collections.Generic;

public sealed partial class RimWorldRunner : AbstractRunner<RimWorldRunner>
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Static Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static readonly List<GameComponent> Components = new();




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override string Name => $"RimWorld Game.{nameof(GetComponent)}<>()";
    public override string TimeComplexity => $"O(n) n:{Components.Count}";




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Protected Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override void Initialize()
    {
        // Note: Worst-case scenario has time complexity of O(n)
        if (Components.Count != 0) return;
        Components.Add(new AlphaComponent()); // Best-case scenario.
        Components.Add(new BetaComponent());
        Components.Add(new AlphaComponent());
        Components.Add(new BetaComponent());
        Components.Add(new AlphaComponent());
        Components.Add(new BetaComponent());
        Components.Add(new GammaComponent()); // Worst-case scenario.
    }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Protected Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected override void RunBest()
    {
        for (int i = 0; i < OperationsPerInvoke; i++)
        {
            Consume(GetComponent<AlphaComponent>());
        }
    }

    protected override void RunWorst()
    {
        for (int i = 0; i < OperationsPerInvoke; i++)
        {
            // Might be even worse in production.
            Consume(GetComponent<GammaComponent>());
        }
    }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private static T GetComponent<T>() where T : GameComponent
    {
        for (int i = 0; i < Components.Count; i++)
        {
            if (Components[i] is T result)
            {
                return result;
            }
        }

        return null;
    }
}
