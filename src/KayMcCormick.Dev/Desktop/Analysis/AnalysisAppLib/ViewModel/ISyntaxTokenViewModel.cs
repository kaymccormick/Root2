using System.Collections.ObjectModel ;
using AnalysisAppLib.XmlDoc.Syntax ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib.XmlDoc.ViewModel
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