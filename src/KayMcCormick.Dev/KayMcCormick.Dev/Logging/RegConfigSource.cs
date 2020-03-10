using JetBrains.Annotations;

namespace KayMcCormick.Dev.Logging
{
    // TODO implement
    // ReSharper disable once UnusedType.Global
    class RegConfigSource : IConfigSource
    {
        public string PathForLogFile(ILogFileSpecification spec) { return ""; }
    }

    internal interface IConfigSource
    {
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedParameter.Global
        string PathForLogFile(ILogFileSpecification spec);



    }

    internal interface ILogEntrySpecification
    {
        // ReSharper disable once UnusedMember.Global
        FunctionalArea GetFunctionalArea();
        // ReSharper disable once UnusedMember.Global
        CrosscuttingConcern GetCrosscuttingConcern();
        // ReSharper disable once UnusedMember.Global
        ExecutionContext GetExecutionContext();
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContext'
    public interface ExecutionContext
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContext'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContext.Application'
        // ReSharper disable once UnusedMember.Global
        Application Application { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContext.Application'
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl'
    // ReSharper disable once UnusedType.Global
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
        // ReSharper disable once UnusedMember.Global
        MainApplication,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Application.MainApplication'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Application.Tests'
        // ReSharper disable once UnusedMember.Global
        Tests,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Application.Tests'
    }

    internal interface ILogFileSpecification : ILogEntrySpecification
    {
    }

    internal enum CrosscuttingConcern
    {


    }

    [UsedImplicitly]
    internal enum FunctionalArea
    {
        [UsedImplicitly] Configuration,
        [UsedImplicitly] UserInterface,
        [UsedImplicitly] DependencyInjection,
        [UsedImplicitly] Model,
        [UsedImplicitly] TypeConversion,
        [UsedImplicitly] Xaml,



    }
}
