namespace KayMcCormick.Dev.Logging
{
    internal interface ILogEntrySpecification
    {
        FunctionalArea GetFunctionalArea ( ) ;

        CrosscuttingConcern GetCrosscuttingConcern ( ) ;

        IExecutionContext GetExecutionContext ( ) ;
    }
}