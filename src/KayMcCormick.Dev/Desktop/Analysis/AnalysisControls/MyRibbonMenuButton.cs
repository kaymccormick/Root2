using System;
using System.Windows;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonMenuButton : RibbonMenuButton, IAppControl
    {
        static MyRibbonMenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonMenuButton),
                new FrameworkPropertyMetadata(typeof(MyRibbonMenuButton)));

        }
        private object _currentItem;

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            var c = base.GetContainerForItemOverride();
            var currentItem = _currentItem;
            _currentItem = null;

            DebugUtils.WriteLine($"container is {c} for {_currentItem}");

            return c;
        }

        /// <inheritdoc />
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            _currentItem = item;
            var br = base.IsItemItsOwnContainerOverride(item);
            if (br == true)
            {
                return true;
            }
            return br;
        }

        public Guid ControlId { get; } = Guid.NewGuid();
    }
}