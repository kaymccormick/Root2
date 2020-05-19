using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using JetBrains.Annotations;

namespace KmDevWpfControls
{
    public sealed class VisualTreeNode : IAsyncExpand, ITreeViewItemCollapse,INotifyPropertyChanged
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
                            Debug.WriteLine(ex.ToString());
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
            if (Items == null)
            {
                throw new InvalidOperationException("Items is nukk");
            }
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
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Collapse()
        {
            IsExpanded = false;
        }
    }
}