#region header
// Kay McCormick (mccor)
// 
// Proj
// ProjInterface
// MyTraceFilter.cs
// 
// 2020-02-20-7:09 PM
// 
// ---
#endregion
using System.Diagnostics ;
using System.Linq ;
using System.Text.RegularExpressions ;
using NLog ;

namespace KayMcCormick.Dev.DataBindingTraceFilter
{
    public class MyTraceFilter : TraceFilter
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        /// <summary>When overridden in a derived class, determines whether the trace listener should trace the event.</summary>
        /// <param name="cache">The <see cref="T:System.Diagnostics.TraceEventCache" /> that contains information for the trace event.</param>
        /// <param name="source">The name of the source.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A trace identifier number.</param>
        /// <param name="formatOrMessage">Either the format to use for writing an array of arguments specified by the <paramref name="args" /> parameter, or a message to write.</param>
        /// <param name="args">An array of argument objects.</param>
        /// <param name="data1">A trace data object.</param>
        /// <param name="data">An array of trace data objects.</param>
        /// <returns>
        /// <see langword="true" /> to trace the specified event; otherwise, <see langword="false" />. </returns>
        public override bool ShouldTrace (
            TraceEventCache cache
          , string          source
          , TraceEventType  eventType
          , int             id
          , string          formatOrMessage
          , object[]        args
          , object          data1
          , object[]        data
        )
        {
            string[] ignores = new[] { "UserOptions." } ;
            var match = Regex.Match ( formatOrMessage , "BindingExpression:(.*) DataItem" ) ;
            if ( match.Success )
            {
                var expr = match.Groups[ 1 ].Captures[ 0 ].Value ;
                
                var haveIgnore = ignores.Any ( s => expr.Contains ( s) ) ;
                Logger.Trace( "{ignore}" , haveIgnore ) ;
                if ( ! haveIgnore )
                {
                    Logger.Debug("expr is {expr}", expr);
                }
                return haveIgnore ;
            }

            return false ;
        }
    }
}