using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace KayMcCormick.Lib.Wpf
{
    public class TableRow0 : Panel
    {
        protected override Visual GetVisualChild(int index)
        {
            return null;
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            DebugUtils.WriteLine($"{visualAdded} {visualRemoved}");
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
DebugUtils.WriteLine($"{e.Property.Name} {e.NewValue} {e.OldValue}");
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            
            DebugUtils.WriteLine($"{oldParent}");
        }

        protected override DependencyObject GetUIParentCore()
        {
            DebugUtils.WriteLine($"GetUIParentcore");
            return base.GetUIParentCore();
        }

        protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
        {
            var parent = VisualTreeHelper.GetParent(this);
            return new TableRowUIElementCollection((UIElement) parent, logicalParent);
        }


        protected override int VisualChildrenCount => 0;
    }

    public class TableRowUIElementCollection : UIElementCollection
    {
        public TableRowUIElementCollection([NotNull] UIElement visualParent, FrameworkElement logicalParent) : base(visualParent, logicalParent)
        {
        }
    }
}