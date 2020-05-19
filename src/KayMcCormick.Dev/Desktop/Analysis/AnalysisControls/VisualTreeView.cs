using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
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
    public class VisualTreeView : Control
    {
        public ObservableCollection<VisualTreeNode> RootItems { get; }

        public VisualTreeView()
        {
            RootItems = new ObservableCollection<VisualTreeNode>();
//            RootItems.Add(new VisualTreeNode{Visual = Window.GetWindow(this)});
            CommandBindings.Add(new CommandBinding(WpfAppCommands.ToggleNodeIsExpanded, OnToggleExecuted));
        }

        private async void OnToggleExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DebugUtils.WriteLine("Received expand node command with param " + e.Parameter);
            try
            {
                if (!(e.Parameter is CustomTreeViewItem cc))
                {
                    DebugUtils.WriteLine("PArameter is not CustomTreeViewItem");
                    return;
                }

                if (cc.IsExpanded)
                    cc.Collapse();
                else
                {
                    SetCurrentValue(Control.CursorProperty, Cursors.Wait);
                    Dispatcher.BeginInvoke(new Action(() => ClearValue(CursorProperty)), DispatcherPriority.ContextIdle, null);
                    await cc.ExpandAsync();
                    DebugUtils.WriteLine("return from expand async");
                }
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
                // ignored
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            RootItems.Clear();
            var window = Window.GetWindow(this);
            RootItems.Add(new VisualTreeNode { Visual = window,
                TransformToSource = window});
        }


        static VisualTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VisualTreeView), new FrameworkPropertyMetadata(typeof(VisualTreeView)));
        }
    }

    public class VisualTreeNode : IAsyncExpand, ITreeViewItemCollapse,INotifyPropertyChanged
    {
        private Visual _visual;
        private DrawingGroup _drawing;
        private bool _isExpanded;
        private ObservableCollection<VisualTreeNode> _items;
        private Rect _contentBounds;
        private Rect _transformedBounds;
        private Visual _transformToSource;
        private Rect? _descendantBounds;
        private GeneralTransform _transform;
        private Type _type;

        public VisualTreeNode()
        {
            
        }

        public Visual Visual
        {
            get { return _visual; }
            set
            {
                _visual = value;
                OnPropertyChanged();
                if (_visual != null)
                {
                    Items = new ObservableCollection<VisualTreeNode>();
                    Items.Add(new VisualTreeNode());
                    Type = _visual.GetType();
                    Visual.Dispatcher.InvokeAsync(() =>
                    {
                        try
                        {
                            var drawingGroup = VisualTreeHelper.GetDrawing(_visual);
                            ContentBounds = VisualTreeHelper.GetContentBounds(_visual);
                            if (TransformToSource != null) Transform_ = Visual.TransformToAncestor(TransformToSource);
                            DescendantBounds =
                                Transform_?.TransformBounds(VisualTreeHelper.GetDescendantBounds(Visual));

                            if (drawingGroup != null)
                            {
                                if (Transform_ != null)
                                {
                                    var t = Transform_.TransformBounds(Drawing1.Bounds);
                                    TransformedBounds = new Rect((int) t.X, (int) t.Y, (int) t.Width, (int) t.Height);
                                }
                            }

                            MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(_visual);
                            Properties_.Clear();
                            if (markupObject != null)
                            {
                                foreach (MarkupProperty mp in markupObject.Properties)
                                {
                                    Properties_.Add(mp);
                                }
                            }

                            
                            Drawing1 = drawingGroup;
                        }
                        catch (Exception ex)
                        {
                            DebugUtils.WriteLine(ex.ToString());
                        }
                    }, DispatcherPriority.DataBind);
                    // LocalValueEnumerator lve = Visual.GetLocalValueEnumerator();
                    // while (lve.MoveNext())
                    // {
                    // Properties_.Add(lve.Current);
                    // }

                    
                    
                }
            }
        }

        public Rect? DescendantBounds
        {
            get { return _descendantBounds; }
            set
            {
                if (Nullable.Equals(value, _descendantBounds)) return;
                _descendantBounds = value;
                OnPropertyChanged();
            }
        }

        public Rect TransformedBounds
        {
            get { return _transformedBounds; }
            set
            {
                if (value.Equals(_transformedBounds)) return;
                _transformedBounds = value;
                OnPropertyChanged();
            }
        }

        public GeneralTransform Transform_
        {
            get { return _transform; }
            set
            {
                if (Equals(value, _transform)) return;
                _transform = value;
                OnPropertyChanged();
            }
        }

        public Visual TransformToSource
        {
            get { return _transformToSource; }
            set
            {
                if (Equals(value, _transformToSource)) return;
                _transformToSource = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MarkupProperty> Properties_ { get; set; } = new ObservableCollection<MarkupProperty>();

        public Rect ContentBounds
        {
            get { return _contentBounds; }
            set
            {
                if (value.Equals(_contentBounds)) return;
                _contentBounds = value;
                OnPropertyChanged();
            }
        }

        public Type Type
        {
            get { return _type; }
            set
            {
                if (Equals(value, _type)) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        public DrawingGroup Drawing1
        {
            get { return _drawing; }
            set
            {
                _drawing = value;
            }
        }

        /// <inheritdoc />
        public Task ExpandAsync()
        {
            Items.Clear();
         var count =    VisualTreeHelper.GetChildrenCount(Visual);
         for(int i = 0; i < count; i++)
         {
             Items.Add(new VisualTreeNode {Visual = VisualTreeHelper.GetChild(Visual, i) as Visual, TransformToSource = this.TransformToSource});
         }

         IsExpanded = true;
         return Task.CompletedTask;
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value == _isExpanded) return;
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<VisualTreeNode> Items
        {
            get { return _items; }
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Collapse()
        {
            IsExpanded = false;
        }
    }

}
