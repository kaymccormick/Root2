using JetBrains.Annotations;
 

namespace KayMcCormick.Dev.Logging
{
    // TODO implement
    // ReSharper disable once UnusedType.Global
    class RegConfigSource
    {
        public string PathForLogFile(ILogFileSpecification spec) { return ""; }
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
