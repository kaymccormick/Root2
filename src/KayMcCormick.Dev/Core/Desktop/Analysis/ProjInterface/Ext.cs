using System.Threading ;

namespace ProjInterface
{
    public static class Ext
    {
        public static SynchronizationContextAwaiter GetAwaiter (
            this SynchronizationContext context
        )
        {
            return new SynchronizationContextAwaiter ( context ) ;
        }
    }
}