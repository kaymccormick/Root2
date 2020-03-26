namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecutionContextImpl : ExecutionContext

    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public ExecutionContextImpl ( Logging.Application application )
        {
            _application = application ;
        }

        private Logging.Application _application ;

        Logging.Application ExecutionContext.Application => _application ;

    }
}
