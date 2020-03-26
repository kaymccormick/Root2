using JetBrains.Annotations;
 

namespace KayMcCormick.Dev.Logging
{
    // TODO implement
    // ReSharper disable once UnusedType.Global
    class RegConfigSource
    {
        public string PathForLogFile(ILogFileSpecification spec) { return ""; }
    }


    

    // ReSharper disable once UnusedType.Global
    public class ExecutionContextImpl : ExecutionContext

    {

        public Application Application { get; set; }

    }


    public enum Application

    {

        // ReSharper disable once UnusedMember.Global
        MainApplication,


        // ReSharper disable once UnusedMember.Global
        Tests,

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
