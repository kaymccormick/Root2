using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using NLog;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonGroup : RibbonGroup
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonControl();
        }

        ItemsControl ParentItemsControl
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this);
            }
        }

    }

}