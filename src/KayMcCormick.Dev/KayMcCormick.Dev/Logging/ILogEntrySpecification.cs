namespace KayMcCormick.Dev.Logging
{
    internal interface ILogEntrySpecification
    {
        // ReSharper disable once UnusedMember.Global
        FunctionalArea GetFunctionalArea();
        // ReSharper disable once UnusedMember.Global
        CrosscuttingConcern GetCrosscuttingConcern();
        // ReSharper disable once UnusedMember.Global
        ExecutionContext GetExecutionContext();
    }
}