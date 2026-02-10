using Eclipse;
using Eclipse.Serialization;

public sealed class EclipseRunner : AbstractRunner<EclipseRunner>
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override string Name => $"Eclipse {nameof(IService)}.{nameof(ISerializationService.Instance)}";
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
            Consume(ISerializationService.Instance);
        }
    }

    protected override void RunWorst()
    {
        for (int i = 0; i < OperationsPerInvoke; i++)
        {
            Consume(ISerializationService.Instance);
        }
    }
}
