public sealed class IdleRunner : AbstractRunner<IdleRunner>
{
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Public Properties
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override string Name => "Idle (Control)";
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
        // Consumes 'i' so compiler will be less likely to optimize this thing out.
        int i = 0;
        for (; i < OperationsPerInvoke; i++) { }
        Consume(i);
    }

    protected override void RunWorst()
    {
        // Consumes 'i' so compiler will be less likely to optimize this thing out.
        int i = 0;
        for (; i < OperationsPerInvoke; i++) { }
        Consume(i);
    }
}
