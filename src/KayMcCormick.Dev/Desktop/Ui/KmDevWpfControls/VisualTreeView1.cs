using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using JetBrains.Annotations;

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
        public static readonly DependencyProperty SelectedVisualProperty = DependencyProperty.Register(
            "SelectedVisual", typeof(Visual), typeof(VisualTreeView1), new PropertyMetadata(default(Visual)));

        public Visual SelectedVisual
        {
            get { return (Visual) GetValue(SelectedVisualProperty); }
            set { SetValue(SelectedVisualProperty, value); }
        }
        public ObservableCollection<VisualTreeNode> InternalRootItems { get; } = new ObservableCollection<VisualTreeNode>();

        public static readonly DependencyProperty RootItemsProperty = DependencyProperty.Register(
            "RootItems", typeof(IEnumerable), typeof(VisualTreeView1), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable RootItems
        {
            get { return (IEnumerable) GetValue(RootItemsProperty); }
            set { SetValue(RootItemsProperty, value); }
        }

        public static readonly DependencyProperty RootVisualProperty = DependencyProperty.Register(
            "RootVisual", typeof(Visual), typeof(VisualTreeView1), new PropertyMetadata(default(Visual)));

        private VisualConverter _vc;

        public Visual RootVisual
        {
            get { return (Visual) GetValue(RootVisualProperty); }
            set { SetValue(RootVisualProperty, value); }
        }
        public VisualTreeView1()
        {
            SetBinding(RootItemsProperty, new Binding("RootVisual") {Source = this,Converter = _vc});
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _vc = TryFindResource("VisualConverter") as VisualConverter;
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var source = DependencyPropertyHelper.GetValueSource(this, RootVisualProperty);
            if (source.IsExpression && source.BaseValueSource == BaseValueSource.Local)
                return;
            if (source.BaseValueSource != BaseValueSource.Default && source.BaseValueSource != BaseValueSource.Local &&
                source.BaseValueSource != BaseValueSource.Inherited)
            {
                return;

            }

            InternalRootItems.Clear();
            var window = Window.GetWindow(this);
            Visual v = window;
            if (window == null)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                for (v = (Visual) VisualParent; VisualTreeHelper.GetParent(v) != null; v = (Visual) VisualTreeHelper.GetParent(v)) ;
            }
            InternalRootItems.Add(new VisualTreeNode { Visual = v,
                TransformToSource = v});
        }


        static VisualTreeView1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VisualTreeView1), new FrameworkPropertyMetadata(typeof(VisualTreeView1)));
        }
    }

    public class VisualTreeViewModel : INotifyPropertyChanged
    {
        private VisualTreeNode _currentVisual;
        private IEnumerable _rootNodes;
        private ObservableCollection<VisualTreeNode> _internalItems = new ObservableCollection<VisualTreeNode>();
        private Visual _rootVisual;

        public VisualTreeNode CurrentVisual
        {
            get { return _currentVisual; }
            set
            {
                if (Equals(value, _currentVisual)) return;
                _currentVisual = value;
                Debug.WriteLine("current visual updated to " + _currentVisual);
                OnPropertyChanged();
            }
        }

        public IEnumerable RootNodes
        {
            get { return InternalItems; }
        }

        public ObservableCollection<VisualTreeNode> InternalItems
        {
            get { return _internalItems; }
            set
            {
                if (Equals(value, _internalItems)) return;
                _internalItems = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RootNodes));
            }
        }

        public Visual RootVisual
        {
            get { return _rootVisual; }
            set
            {
                if (Equals(value, _rootVisual)) return;
                _rootVisual = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}