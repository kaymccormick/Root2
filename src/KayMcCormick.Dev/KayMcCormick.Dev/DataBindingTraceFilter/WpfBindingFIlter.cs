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
using JetBrains.Annotations ;
using NLog ;

namespace KayMcCormick.Dev.DataBindingTraceFilter
{
    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class WpfBindingFilter : TraceFilter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="formatOrMessage"></param>
        /// <param name="args"></param>
        /// <param name="data1"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool ShouldTrace (
            TraceEventCache cache
          , string          source
          , TraceEventType  eventType
          , int             id
          , [ JetBrains.Annotations.NotNull ] string          formatOrMessage
          , object[]        args
          , object          data1
          , object[]        data
        )
        {
            string[] ignores = { "UserOptions." } ;
            var match = Regex.Match ( formatOrMessage , "BindingExpression:(.*) DataItem" ) ;
            if ( ! match.Success )
            {
                return false ;
            }

            var expr = match.Groups[ 1 ].Captures[ 0 ].Value ;

            var haveIgnore = ignores.Any ( s => expr.Contains ( s ) ) ;

            Logger.Trace ( @"{ignore}" , haveIgnore ) ;

            if ( ! haveIgnore )
            {
                Logger.Debug ( @"expr is {expr}" , expr ) ;
            }

            return haveIgnore ;

        }
    }
}