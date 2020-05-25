using System;
using System.Windows;
using System.Windows.Controls;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    public class MyGrid : Grid
    {
        /// <inheritdoc />
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            DebugUtils.WriteLine($"{visualAdded} {visualRemoved}");
        }

        /// <inheritdoc />
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            DebugUtils.WriteLine($"{oldParent} {VisualParent}");
        }

        /// <inheritdoc />
        public override void BeginInit()
        {
            base.BeginInit();
        }

        /// <inheritdoc />
        public override void EndInit()
        {
            base.EndInit();
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
    }
}