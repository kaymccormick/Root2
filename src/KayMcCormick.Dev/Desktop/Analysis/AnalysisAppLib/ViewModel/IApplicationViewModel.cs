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
using AnalysisAppLib.Syntax ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib.ViewModel
{
    public interface IApplicationViewModel : IViewModel
    {
        ObservableCollection < SyntaxItem > SyntaxItems { get ; }
    }
}