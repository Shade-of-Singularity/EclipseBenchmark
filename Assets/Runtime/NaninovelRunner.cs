using Eclipse.Serialization;
using Naninovel;

public sealed class NaninovelRunner : AbstractRunner<NaninovelRunner>
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override string Name => $"Naninovel {nameof(Engine)}.{nameof(Engine.GetService)}<>()";
    public override string TimeComplexity => "O(1)";




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override void Initialize() { }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Protected Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    protected override void RunBest()
    {
        for (int i = 0; i < OperationsPerInvoke; i++)
        {
            Consume(Engine.GetService<IUIManager>());
        }
    }

    protected override void RunWorst()
    {
        for (int i = 0; i < OperationsPerInvoke; i++)
        {
            Consume(Engine.GetService<IUIManager>()); // Note: Is there even a worst-case with dictionaries?
        }
    }
}
