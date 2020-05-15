using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonTabHeader : RibbonTabHeader
    {
        static MyRibbonTabHeader()
        {
            var type = typeof(MyRibbonTabHeader);
            UIElement.VisibilityProperty.OverrideMetadata(type, (PropertyMetadata)new FrameworkPropertyMetadata((PropertyChangedCallback)null, new CoerceValueCallback(MyRibbonTabHeader.CoerceVisibility)));
        }

        private static object CoerceVisibility(DependencyObject d, object basevalue)
        {
            MyRibbonTab ribbonTab = ((MyRibbonTabHeader)d).MyRibbonTab;
            var ribbonTabVisibility = ribbonTab?.Visibility;
            DebugUtils.WriteLine($"baseValue = {basevalue}; ribbonTabVisibility = {ribbonTabVisibility}");
            return ribbonTab != null ? (object)ribbonTabVisibility : basevalue;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        internal MyRibbonTab MyRibbonTab
        {
            get
            {
                ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer((DependencyObject)this);
                System.Windows.Controls.Ribbon.Ribbon ribbon = this.Ribbon;
                if (itemsControl == null || ribbon == null)
                    return null;
                int index = itemsControl.ItemContainerGenerator.IndexFromContainer((DependencyObject)this);
                return ribbon.ItemContainerGenerator.ContainerFromIndex(index) as MyRibbonTab;
            }
        }
    }
}