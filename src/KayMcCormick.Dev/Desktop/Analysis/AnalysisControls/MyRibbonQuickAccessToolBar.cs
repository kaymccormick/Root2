using System.Collections;
using System.Windows;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <inheritdoc />
    public class MyRibbonQuickAccessToolBar : RibbonQuickAccessToolBar
    {
        public override void OnApplyTemplate()
        {
            DebugUtils.WriteLine($"{this}.{nameof(OnApplyTemplate)}");
            base.OnApplyTemplate();
        }

        static MyRibbonQuickAccessToolBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonQuickAccessToolBar),
                new FrameworkPropertyMetadata(typeof(MyRibbonQuickAccessToolBar)));


        }

        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return (DependencyObject)new MyRibbonControl();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }
        internal bool ContainsId(object targetID)
        {
            foreach (object obj in (IEnumerable)this.Items)
            {
                if (obj is DependencyObject element && object.Equals(RibbonControlService.GetQuickAccessToolBarId(element), targetID))
                    return true;
            }
            return false;
        }
        protected override Size MeasureOverride(Size constraint)
        {
            DebugUtils.WriteLine($"{this}.{nameof(MeasureOverride)}");
            return base.MeasureOverride(constraint);
        }

    }
}