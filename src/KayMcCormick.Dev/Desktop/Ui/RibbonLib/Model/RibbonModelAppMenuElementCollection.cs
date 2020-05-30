using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace RibbonLib.Model
{
    /// <inheritdoc />
    [ContentWrapper(typeof(RibbonModelAppMenuItem))]
    [ContentWrapper(typeof(RibbonModelAppSplitMenuItem))]
    [ContentWrapper(typeof(RibbonModelApplicationMenu))]
    public class RibbonModelAppMenuElementCollection : ObservableCollection<RibbonModelAppMenuElement>
    {
    }
}