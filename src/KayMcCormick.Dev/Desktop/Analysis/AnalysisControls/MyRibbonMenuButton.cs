using System.Windows;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    public class MyRibbonMenuButton : RibbonMenuButton
    {
        private object _currentItem;

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            var c = base.GetContainerForItemOverride();
            var currentItem = _currentItem;
            _currentItem = null;

            DebugUtils.WriteLine($"container is {c}");

            return c;
        }

        /// <inheritdoc />
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            var br = base.IsItemItsOwnContainerOverride(item);
            if (br == true)
            {
                return true;
            }
            _currentItem = item;
            return br;
        }
    }
}