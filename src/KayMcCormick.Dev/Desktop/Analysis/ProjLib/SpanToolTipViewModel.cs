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
using Microsoft.CodeAnalysis ;

namespace ProjLib
{
    internal class SpanToolTipViewModel : ISpanToolTipViewModel
    {
        public List < object > Spans { get ; set ; } = new List < object > ();

        public Location Location { get ; set ; }
    }
}