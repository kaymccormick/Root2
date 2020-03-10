﻿#region header
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
            Logger.Info ( "{disp}" , _displayString ) ;
        }

        private Func < object , object > _getResource ;
        private string                   _displayString ;
    }
}