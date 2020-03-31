using System.Collections.ObjectModel ;
using AnalysisAppLib.Syntax ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISyntaxTokenViewModel : IViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        ObservableCollection < SyntaxItem > SyntaxItems { get ; }
    }
}