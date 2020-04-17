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
using FindLogUsages ;
using Microsoft.CodeAnalysis.Text ;
using NLog ;

namespace AnalysisAppLib.Span
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class LogInvocationSpan : SpanObject < ILogInvocation > , ISpanObject < ILogInvocation >
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;


        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly string _displayString ;


#pragma warning disable 169
        private Func < object , object > _getResource ;
#pragma warning restore 169

        /// <summary>
        /// 
        /// </summary>
        /// <param name="span"></param>
        /// <param name="instance"></param>
        public LogInvocationSpan ( TextSpan span , ILogInvocation instance ) : base (
                                                                                     span
                                                                                   , instance
                                                                                    )
        {
            _displayString = Instance.MethodDisplayName
                             + " "
                             + string.Join (
                                            ", "
                                          , Instance.Arguments.Select (
                                                                       ( argument , i )
                                                                           => argument.GetJSON (
                                                                                                argument
                                                                                               )
                                                                      )
                                           ) ;

            Logger.Info ( "{disp}" , _displayString ) ;
        }
    }
}