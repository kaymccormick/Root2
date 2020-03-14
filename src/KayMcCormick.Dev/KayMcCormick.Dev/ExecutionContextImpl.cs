using KayMcCormick.Dev.Logging ;

namespace KayMcCormick.Dev
{
    public class ExecutionContextImpl : ExecutionContext
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl.Application'
        Logging.Application ExecutionContext.Application { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExecutionContextImpl.Application'
    }
}