#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// SpanTT.cs
// 
// 2020-02-26-11:09 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Windows.Controls ;
using Microsoft.CodeAnalysis ;

namespace ProjLib
{
    internal class SpanTT : ToolTip
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ToolTip" /> class. </summary>
        public SpanTT ( SpanToolTip content ) { Content = CustomToolTip = content ; }

        public SpanToolTip CustomToolTip { get ; set ; }

        public ISpanToolTipViewModel ViewModel { get ; set ; }

    }
}