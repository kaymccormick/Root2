using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    [ContentWrapper(typeof(RibbonModelButton))]
    public class RibbonModelGroupItemCollection : ObservableCollection<RibbonModelItem>
    {
    }
}