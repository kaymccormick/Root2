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
    public static class AnalysisCommands
    {
        public static RoutedUICommand AnalyzeControlFlow = new RoutedUICommand("Analyze Control Flow", nameof(AnalyzeControlFlow), typeof(AnalysisCommands));
        public static RoutedUICommand GetDeclaredSymbol= new RoutedUICommand("Get Declared Symbol", nameof(GetDeclaredSymbol), typeof(AnalysisCommands));
    }
}