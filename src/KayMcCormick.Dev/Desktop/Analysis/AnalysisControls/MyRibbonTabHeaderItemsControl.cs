using System.Windows;
using System.Windows.Controls.Ribbon;

namespace AnalysisControls
{
    public class MyRibbonTabHeaderItemsControl : RibbonTabHeaderItemsControl
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonTabHeader();
        }
    }
}