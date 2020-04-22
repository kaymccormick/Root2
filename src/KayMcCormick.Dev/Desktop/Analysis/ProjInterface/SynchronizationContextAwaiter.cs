using System ;
using System.Runtime.CompilerServices ;
using System.Threading ;

namespace ProjInterface
{
    // ReSharper disable once StructCanBeMadeReadOnly
    public struct SynchronizationContextAwaiter : INotifyCompletion
    {
        private static readonly SendOrPostCallback
            PostCallback = state => ( ( Action ) state ) ( ) ;

        private readonly SynchronizationContext _context ;

        public SynchronizationContextAwaiter ( SynchronizationContext context )
        {
            _context = context ;
        }

        // ReSharper disable once UnusedMember.Global
        public bool IsCompleted { get { return _context == SynchronizationContext.Current ; } }

        public void OnCompleted ( Action continuation )
            => _context.Post ( PostCallback , continuation ) ;

        public void GetResult ( ) { }
    }
}