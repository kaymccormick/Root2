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
    public interface ISpanViewModel
    {
        object   getInstance();
        TextSpan Span { get; }

    }
}