using KayMcCormick.Dev.Logging ;

namespace KayMcCormick.Dev
{
    public class ExecutionContextImpl : ExecutionContext
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl'
    {
        public ExecutionContextImpl ( Logging.Application application )
        {
            _application = application ;
        }

        private Logging.Application _application ;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl.Application'
        Logging.Application ExecutionContext.Application => _application ;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl.Application'
    }
}