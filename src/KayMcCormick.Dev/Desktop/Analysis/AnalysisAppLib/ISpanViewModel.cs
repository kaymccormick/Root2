#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// ISpanViewModel.cs
// 
// 2020-02-27-1:56 AM
// 
// ---
#endregion
using Microsoft.CodeAnalysis.Text ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISpanViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object   getInstance();
        /// <summary>
        /// 
        /// </summary>
        TextSpan Span { get; }

    }
}