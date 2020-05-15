using System;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonContextualTabGroupItemsControl : RibbonContextualTabGroupItemsControl, IAppControl
    {
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            RibbonDebugUtils.OnPropertyChanged(this.ToString(), this, e);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonContextualTabGroup();
        }
        public Guid ControlId { get; } = Guid.NewGuid();

    }
}