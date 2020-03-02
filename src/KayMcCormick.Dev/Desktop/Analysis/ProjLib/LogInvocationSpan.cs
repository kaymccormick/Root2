#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// LogInvocationSpan.cs
// 
// 2020-02-26-10:00 PM
// 
// ---
#endregion
using System ;
using System.Linq ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using AnalysisFramework ;

using Microsoft.CodeAnalysis.Text ;
using NLog ;

namespace ProjLib
{
    public class LogInvocationSpan : SpanObject < LogInvocation >, ISpanObject <LogInvocation>
    {
        private static Logger Logger = Logger ;
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public LogInvocationSpan (
            TextSpan                 span
          , LogInvocation            instance
        ) : base ( span , instance )
        {
            _displayString = Instance.MethodDisplayName
                             + " "
                             + string.Join (
                                            ", "
                                          , Instance.Arguments.Select (
                                                                       ( argument , i )
                                                                           => argument.JSON
                                                                      )
                                           ) ;
            Logger.Info ( "{disp}" , _displayString ) ;
        }

        private Func < object , object > _getResource ;
        private string                   _displayString ;
    }
}