using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace KmDevWpfControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls;assembly=AnalysisControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:VisualTreeView/>
    ///
    /// </summary>
    public class VisualTreeView1 : Control
    {
        public ObservableCollection<VisualTreeNode> RootItems { get; }

        public VisualTreeView1()
        {
            RootItems = new ObservableCollection<VisualTreeNode>();
//            RootItems.Add(new VisualTreeNode{Visual = Window.GetWindow(this)});
            CommandBindings.Add(new CommandBinding(CustomTreeView.ToggleNodeIsExpanded, OnToggleExecuted));
        }

        private async void OnToggleExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Debug.WriteLine("Received expand node command with param " + e.Parameter);
            try
            {
                if (!(e.Parameter is CustomTreeViewItem cc))
                {
                    Debug.WriteLine("PArameter is not CustomTreeViewItem");
                    return;
                }

                if (cc.IsExpanded)
                    cc.Collapse();
                else
                {
                    SetCurrentValue(Control.CursorProperty, Cursors.Wait);
                    Dispatcher.BeginInvoke(new Action(() => ClearValue(CursorProperty)), DispatcherPriority.ContextIdle, null);
                    await cc.ExpandAsync();
                    Debug.WriteLine("return from expand async");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                // ignored
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            RootItems.Clear();
            var window = Window.GetWindow(this);
            Visual v = window;
            if (window == null)
            {
                for (v = (Visual) VisualParent; VisualTreeHelper.GetParent(v) != null; v = (Visual) VisualTreeHelper.GetParent(v)) ;
            }
            RootItems.Add(new VisualTreeNode { Visual = v,
                TransformToSource = v});
        }


        static VisualTreeView1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VisualTreeView1), new FrameworkPropertyMetadata(typeof(VisualTreeView1)));
        }
    }
}