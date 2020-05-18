using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Markup;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    [Editor(typeof(GroupItemCollectionEditor), typeof(CollectionEditor))]
    [ContentWrapper(typeof(RibbonModelItemButton))]
    public class RibbonModelGroupItemCollection : ObservableCollection<RibbonModelItem>
    {
    }
}