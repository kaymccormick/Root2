#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjInterface
// BreakTraceListener.cs
// 
// 2020-03-08-7:56 PM
// 
// ---
#endregion
using System.Diagnostics ;

namespace ProjInterface
{
    public class BreakTraceListener : TraceListener
    {
        private bool _doBreak ;

        /// <summary>When overridden in a derived class, writes the specified message to the listener you create in the derived class.</summary>
        /// <param name="message">A message to write. </param>
        public override void Write ( string message )
        {
            if ( DoBreak )
            {
                Debugger.Break ( ) ;
            }
        }

        /// <summary>When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.</summary>
        /// <param name="message">A message to write. </param>
        public override void WriteLine ( string message )
        {
            if ( DoBreak ) { Debugger.Break ( ) ; }
        }

        public bool DoBreak { get => _doBreak ; set => _doBreak = value ; }
    }
}