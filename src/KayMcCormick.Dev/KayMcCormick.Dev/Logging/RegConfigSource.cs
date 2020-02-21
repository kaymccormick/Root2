namespace KayMcCormick.Dev.Logging
{
    class RegConfigSource : IConfigSource
    {
        public string PathForLogFile(ILogFileSpecification spec) { return ""; }
    }

    internal interface IConfigSource
    {
        string PathForLogFile(ILogFileSpecification spec);



    }

    internal interface ILogEntrySpecification
    {
        FunctionalArea GetFunctionalArea();
        CrosscuttingConcern GetCrosscuttingConcern();
        ExecutionContext GetExecutionContext();
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContext'
    public interface ExecutionContext
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContext'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContext.Application'
        Application Application { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContext.Application'
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl'
    public class ExecutionContextImpl : ExecutionContext
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl.Application'
        public Application Application { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl.Application'
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Application'
    public enum Application
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Application'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Application.MainApplication'
        MainApplication,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Application.MainApplication'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Application.Tests'
        Tests,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Application.Tests'
    }

    internal interface ILogFileSpecification : ILogEntrySpecification
    {
    }

    internal enum CrosscuttingConcern
    {


    }

    internal enum FunctionalArea
    {
        Configuration,
        UserInterface,
        DependencyInjection,
        Model,
        TypeConversion,
        Xaml,



    }
}
