namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    ///     Execution context of application.
    /// </summary>
    internal interface ExecutionContext
    {
        /// <summary>
        ///     Application execution context.
        /// </summary>
        Application Application { get ; }
    }
}