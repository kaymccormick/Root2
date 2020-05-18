using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace AnalysisControls.RibbonModel
{
    /// <inheritdoc />
    [ContentWrapper(typeof(RibbonModelAppMenuItem))]
    [ContentWrapper(typeof(RibbonModelAppSplitMenuItem))]
    [ContentWrapper(typeof(RibbonModelApplicationMenu))]
    public class RibbonModelAppMenuElementCollection : ObservableCollection<RibbonModelAppMenuElement>
    {
    }
}