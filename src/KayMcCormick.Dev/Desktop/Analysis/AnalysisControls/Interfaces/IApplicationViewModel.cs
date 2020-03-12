#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// IApplicationViewModel.cs
// 
// 2020-03-11-6:58 PM
// 
// ---
#endregion
using System.Collections.ObjectModel ;

namespace AnalysisControls.Interfaces
{
    public interface IApplicationViewModel
    {
        ObservableCollection < SyntaxItem > SyntaxItems { get ; }
    }
}