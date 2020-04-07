﻿#region header
// Kay McCormick (mccor)
// 
// ManagedProd
// AnalysisControls
// AnalysisCommands.cs
// 
// 2020-03-03-3:38 PM
// 
// ---
#endregion
using System.Windows.Input ;

namespace AnalysisControls
{
    // ReSharper disable once UnusedType.Global
    /// <summary>
    /// 
    /// </summary>
    public static class AnalysisCommands
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedUICommand AnalyzeControlFlow =
            new RoutedUICommand (
                                 "Analyze Control Flow"
                               , nameof ( AnalyzeControlFlow )
                               , typeof ( AnalysisCommands )
                                ) ;

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedUICommand GetDeclaredSymbol =
            new RoutedUICommand (
                                 "Get Declared Symbol"
                               , nameof ( GetDeclaredSymbol )
                               , typeof ( AnalysisCommands )
                                ) ;
    }
}