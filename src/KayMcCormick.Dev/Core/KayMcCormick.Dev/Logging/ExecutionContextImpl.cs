namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// </summary>
    public sealed class ExecutionContextImpl : ExecutionContext

    {
        private readonly Application _application ;

        /// <summary>
        /// </summary>
        /// <param name="application"></param>
        public ExecutionContextImpl ( Application application ) { _application = application ; }

        Application ExecutionContext.Application { get { return _application ; } }
    }
}