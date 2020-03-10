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
using AnalysisFramework ;

using Microsoft.CodeAnalysis.Text ;
using NLog ;

namespace ProjLib
{
    public class LogInvocationSpan : SpanObject < ILogInvocation >, ISpanObject <ILogInvocation>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger() ;
        public LogInvocationSpan (
            TextSpan                 span
          , ILogInvocation            instance
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
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            Logger.Info ( "{disp}" , _displayString ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        }

        private Func < object , object > _getResource ;
        private string                   _displayString ;
    }
}