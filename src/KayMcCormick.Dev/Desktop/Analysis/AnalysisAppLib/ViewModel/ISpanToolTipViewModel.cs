#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// ISpanToolTipViewModel.cs
// 
// 2020-02-26-11:24 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface  ISpanToolTipViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        List < object > Spans { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        Location Location { get ; set ; }
    }
}