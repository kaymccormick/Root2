using System.Windows;
using System.Windows.Controls.Ribbon;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonTabHeaderItemsControl : RibbonTabHeaderItemsControl
    {
        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonTabHeader();
        }
    }
}