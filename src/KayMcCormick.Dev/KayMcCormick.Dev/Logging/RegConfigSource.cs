using JetBrains.Annotations;
 

namespace KayMcCormick.Dev.Logging
{
    // TODO implement
    
    class RegConfigSource
    {
        public string PathForLogFile(ILogFileSpecification spec) { return ""; }
    }


    

    
    public class ExecutionContextImpl : ExecutionContext

    {

        public Application Application { get; set; }

    }


    public enum Application

    {

        
        MainApplication,


        
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
