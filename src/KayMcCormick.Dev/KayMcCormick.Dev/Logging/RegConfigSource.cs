namespace KayMcCormick.Dev.Logging
{
    // TODO implement

    internal class RegConfigSource
    {
    }


    // internal class ExecutionContextImpl : ExecutionContext
    //
    // {
    //     public Application Application { get ; set ; }
    // }


    /// <summary>
    ///     Enumeration
    /// </summary>
    public enum Application
    {
        /// <summary>
        ///     Application
        /// </summary>
        MainApplication

       ,


        /// <summary>
        ///     Tests
        /// </summary>
        Tests
    }

    internal interface ILogFileSpecification : ILogEntrySpecification
    {
    }

    internal enum CrosscuttingConcern { }


    internal enum FunctionalArea
    {
        Configuration , UserInterface , DependencyInjection , Model , TypeConversion , Xaml
    }
}