using System;
using System.Windows;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;
using NLog;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonContextualTabGroupItemsControl : RibbonContextualTabGroupItemsControl, IAppControl
    {
        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            MyRibbon = (MyRibbon)TemplatedParent;
        }

        internal MyRibbonContextualTabGroup FindHeader(object content)
        {
            int count = this.Items.Count;
            for (int index = 0; index < count; ++index)
            {
                var container = this.ItemContainerGenerator.ContainerFromIndex(index);
                DebugUtils.WriteLine($"container is {container}");
                if (container is MyRibbonContextualTabGroup contextualTabGroup)
                {
                    if (object.Equals(contextualTabGroup.Header, content))
                    {
                        return contextualTabGroup;
                    }
                }
            }

            return (MyRibbonContextualTabGroup)null;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            RibbonDebugUtils.OnPropertyChanged(this.ToString(), this, e);
        }

        public MyRibbonContextualTabGroupItemsControl()
        {
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonContextualTabGroup() {MyRibbon = MyRibbon, Logger = Logger};
        }

        public MyRibbon MyRibbon { get; set; }

        public Guid ControlId { get; } = Guid.NewGuid();
        public Logger Logger { get; set; }
    }
}