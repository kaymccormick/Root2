using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelItemSplitButton : RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual  ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();

        /// <inheritdoc />
        public override ControlKind Kind => ControlKind.RibbonSplitButton;
    }
}