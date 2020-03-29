#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// SpanToolTipViewModel.cs
// 
// 2020-02-26-11:25 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using AnalysisAppLib.ViewModel ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib
{
    internal class SpanToolTipViewModel : ISpanToolTipViewModel
    {
        public List < object > Spans { get ; set ; } = new List < object > ( ) ;

        public Location Location { get ; set ; }
    }
}