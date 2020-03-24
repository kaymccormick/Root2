using System.Collections.ObjectModel ;
using AnalysisAppLib.Syntax ;

namespace AnalysisAppLib.ViewModel
{
    public interface ISyntaxTokenViewModel : IViewModel
    {
        ObservableCollection<SyntaxItem> SyntaxItems { get; }
    }
}