using System.Collections.ObjectModel ;
using AnalysisAppLib.Syntax ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib.ViewModel
{
    public interface ISyntaxTokenViewModel : IViewModel
    {
        ObservableCollection < SyntaxItem > SyntaxItems { get ; }
    }
}