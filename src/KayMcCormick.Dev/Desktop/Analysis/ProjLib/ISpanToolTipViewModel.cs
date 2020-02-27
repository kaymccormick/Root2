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

namespace ProjLib
{
    internal interface ISpanToolTipViewModel
    {
        List < object > Spans { get ; set ; }

        Location Location { get ; set ; }
    }
}