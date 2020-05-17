using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace KayMcCormick.Lib.Wpf
{
    public class TablePanel2 : Panel
    {
        public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
            "ColumnSpacing", typeof(double), typeof(TablePanel), new PropertyMetadata(default(double), OnColumnSpacingUpdated));

        protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
        {
            return new TableElementCollection(this, logicalParent);
        }

        public IEnumerable<UIElement> TableContents { get; } = new List<UIElement>();
        private static void OnColumnSpacingUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TablePanel t = (TablePanel)d;
            t.InvalidateMeasure();
        }

        public double ColumnSpacing
        {
            get { return (double) GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
            "RowSpacing", typeof(double), typeof(TablePanel), new PropertyMetadata(default(double), InRowSpacingUpdated));

        private static void InRowSpacingUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TablePanel t = (TablePanel)d;
            t.InvalidateMeasure();
        }

        public double RowSpacing
        {
            get { return (double) GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }
        private List<Rect> _rects = new List<Rect>(20);
        private Size _cellAvailableSize;

        public static readonly DependencyProperty NumColumnsProperty = DependencyProperty.Register(
            "NumColumns", typeof(int), typeof(TablePanel), new PropertyMetadata(2, OnNumColumnsUpdated));

        private static void OnNumColumnsUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TablePanel t = (TablePanel)d;
            t.InvalidateMeasure();
        }

        public int NumColumns
        {
            get { return (int) GetValue(NumColumnsProperty); }
            set { SetValue(NumColumnsProperty, value); }
        }
        public TablePanel2()
        {
            
        }

        List<double> rowHeights = new List<double>(25);
        private List<double> columnWidths = new List<double>(3);

        public override void OnApplyTemplate()
        {
            
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var length = VisualChildrenCount;
            List<UIElement> elem = new List<UIElement>();
            List<UIElement> elem2 = new List<UIElement>();
            int nrows = 0;
            int cellNum = 0;
            for (int j = 0; j < length; j++)
            {
                var visualChild = GetVisualChild(j);
                elem.Add((UIElement) visualChild);
                // if (visualChild is TableRow tr)
                // {
                    // for (int k = 0; k < NumColumns; k++)
                    // {
                        // elem2.Add(tr.Children[k]);
                    // }

                    // if (cellNum % NumColumns == 0)
                    // {

                    // }
                    // else
                    // {
                        // cellNum += NumColumns - (cellNum % NumColumns);
                        // elem2.AddRange(Enumerable.Repeat<UIElement>(null, NumColumns - (cellNum % NumColumns)));
                    // }

                    
                    // cellNum += NumColumns;
                // }
                // else
                {
                    cellNum++;
                    elem2.Add((UIElement) visualChild);
                }
            }

            var rows = cellNum / NumColumns + cellNum % NumColumns == 0 ? 0 : 1;
            var rowHeightsCount = rows - rowHeights.Count;
            if(rowHeightsCount>0)
            rowHeights.AddRange(Enumerable.Repeat(0.0, rowHeightsCount));
            var columnWidthsCount = NumColumns - columnWidths.Count;
            if(columnWidthsCount > 0)
            columnWidths.AddRange(Enumerable.Repeat(0.0, columnWidthsCount));
            int col = 0;
            int row = 0;
            double rowHeight = 0.0;
            ;
            double totalHeight = 0.0;
            double Xpos = 0.0;
            double Ypos = 0.0;
            var i = 0;
            var avail = new Size(availableSize.Width - ColumnSpacing * NumColumns, availableSize.Height - RowSpacing * rows);
            _cellAvailableSize = new Size(avail.Width / NumColumns, avail.Height / rows);
            
            for(i = 0; i < elem2.Count; i++)
            {
                UIElement internalChild = elem2[i];
                if (internalChild != null)
                {


                    internalChild.Measure(new Size(_cellAvailableSize.Width, Double.MaxValue));
                    var d1 = internalChild.DesiredSize;
                    DebugUtils.WriteLine($"[{i}] {d1}");
                    internalChild.Measure(_cellAvailableSize);

                    rowHeight = Math.Max(Math.Max(rowHeight, internalChild.DesiredSize.Height), d1.Height);
                    if (internalChild.DesiredSize.Width > columnWidths[col])
                    {
                        columnWidths[col] = internalChild.DesiredSize.Width;
                    }
                }

                // _rects[i] = new Rect(col == 0? 0:columnWidths[col - 1], Ypos, columnWidths[col], rowHeight);
                col = (col + 1) % NumColumns;
                if (col == 0)
                {
                    totalHeight += rowHeight;
                    Ypos += rowHeight + RowSpacing;
                    rowHeights[row] = rowHeight;
                    row++;
                }

            }

            if (col != 0)
            {
                totalHeight += rowHeight;
                Ypos += rowHeight + RowSpacing;
                rowHeights[row] = rowHeight;
                row++;
            }

            var width = columnWidths.Sum() + NumColumns * ColumnSpacing;
            return new Size(width, Ypos);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var length = VisualChildrenCount;
            
            var rows = length / NumColumns + (length % NumColumns == 0 ? 0 : 1);
            
            int col = 0;
            int row = 0;
            double rowHeight = 0.0;
            var totalHEight = rowHeights.Sum() + RowSpacing * rows;
            double excessPerRow = 0.0;
            if (totalHEight < finalSize.Height)
            {
                excessPerRow = (finalSize.Height - totalHEight) / rows;
                totalHEight = finalSize.Height;
            }

            double totalWidth = columnWidths.Sum() + ColumnSpacing * 2;
            IEnumerable<double> excessWidthPerCol = Enumerable.Repeat(0.0, columnWidths.Count);
            if(totalWidth < finalSize.Width)
            {
                var excessWidth = finalSize.Width - totalWidth;
                excessWidthPerCol = columnWidths.Select(c => c / totalWidth * excessWidth);
                totalWidth = finalSize.Width;
            }
            double Xpos = 0.0;
            double Ypos = 0.0;
            var i = 0;
            var x = excessWidthPerCol.ToArray();
            for(i = 0; i < length; i++)
            {
                var internalChild =(UIElement) GetVisualChild(i);
                double width = columnWidths[col] + x[col] ;
                double height = rowHeights[row] + excessPerRow;
                var finalRect = new Rect(Xpos, Ypos, width, height);
                internalChild.Arrange(finalRect);
                DebugUtils.WriteLine(finalRect.ToString());

                col = (col + 1) % NumColumns;
                if (col == 0)
                {
                    row++;
                    Ypos += height + RowSpacing;
                    Xpos = 0;
                }
                else
                {
                    Xpos += width + ColumnSpacing;
                }

            }
            if (col != 0)
            {
                
                Ypos += rowHeights[row] + excessPerRow + RowSpacing;
                row++;
                Xpos = 0;
            }

            return new Size(totalWidth, Ypos);
        }

        // protected override Visual GetVisualChild(int index)
        // {
            // DebugUtils.WriteLine($"{nameof(GetVisualChild)} {index}");
            // int num = 0;
            // foreach (var c in InternalChildren)
            // {
                // if (c is TableRow r)
                // {
                    // foreach (var d in r.Children)
                    // {
                        // if (index == num)
                        // {
                            // return (Visual)d;
                        // }
                        // num++;
                    // }
                // }
                // else
                // {
                    // if (index == num)
                    // {
                        // return (Visual)c;
                    // }
                    // num++;
                // }
            // }

            // return null;

        // }

        // protected override int VisualChildrenCount
        // {
            // get
            // {
                // int num = 0;
                // foreach (var c in InternalChildren)
                // {
                    // if (c is TableRow r)
                    // {
                        // foreach (var d in r.Children)
                        // {
                            // num++;
                        // }
                    // }
                    // else
                    // {
                        // num++;
                    // }
                // }

                // DebugUtils.WriteLine($"{nameof(VisualChildrenCount)} {num}");
                // return num;
            
        // }
        // }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            DebugUtils.WriteLine($"{visualAdded} {visualRemoved}");
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
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

        protected override IEnumerator LogicalChildren
        {
            get
            {
                var r = base.LogicalChildren;
                while(r.MoveNext())
                    DebugUtils.WriteLine(r.Current.ToString());
                r.Reset();
                return r;
            }
        }
    }

    public class TableElementCollection : UIElementCollection
    {
        private VisualCollection _visualCollection;

        public TableElementCollection([NotNull] UIElement visualParent, FrameworkElement logicalParent) : base(
            visualParent, logicalParent)
        {
            _visualCollection = new VisualCollection(visualParent);
        }

        public override void CopyTo(Array array, int index)
        {
            _visualCollection.CopyTo(array, index);
        }

        public override void CopyTo(UIElement[] array, int index)
        {
            _visualCollection.CopyTo(array, index);
        }

        public override int Add(UIElement element)
        {
            // if (element is TableRow tr)
            // {
                // foreach (UIElement trChild in tr.Children)
                // {
                    // _visualCollection.Add(element);
                // }
            // }
            // else
            {
                return _visualCollection.Add(element);
            }

            return 0;
        }

        public override int IndexOf(UIElement element)
        {
            return _visualCollection.IndexOf(element);
        }

        public override void Remove(UIElement element)
        {
            _visualCollection.Remove(element);
        }

        public override bool Contains(UIElement element)
        {
            return _visualCollection.Contains(element);
        }

        public override void Clear()
        {
            _visualCollection.Clear();
        }

        public override void Insert(int index, UIElement element)
        {
            throw new NotImplementedException();
            _visualCollection.Insert(index, element);

        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
            _visualCollection.RemoveAt(index);
        }

        public override void RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
            _visualCollection.RemoveRange(index, count);
        }

        public override IEnumerator GetEnumerator()
        {
            return _visualCollection.GetEnumerator();
        }

        public override int Count
        {
            get { return _visualCollection.Count; }
        }

        public override bool IsSynchronized
        {
            get { return _visualCollection.IsSynchronized; }
        }

        public override object SyncRoot
        {
            get { return _visualCollection.SyncRoot; }
        }

        public override int Capacity
        {
            get { return _visualCollection.Capacity; }
            set { _visualCollection.Capacity = value; }
        }

        public override UIElement this[int index]
        {
            get { return (UIElement) _visualCollection[index]; }
            set { _visualCollection[index] = value; }
        }

        public override string ToString()
        {
            return _visualCollection.ToString();
        }

        public override bool Equals(object obj)
        {
            return _visualCollection.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _visualCollection.GetHashCode();
        }
    }

    public class TablePanel : Panel
        {
            public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
                "ColumnSpacing", typeof(double), typeof(TablePanel), new PropertyMetadata(default(double), OnColumnSpacingUpdated));

            private static void OnColumnSpacingUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                TablePanel t = (TablePanel)d;
                t.InvalidateMeasure();
            }

            public double ColumnSpacing
            {
                get { return (double)GetValue(ColumnSpacingProperty); }
                set { SetValue(ColumnSpacingProperty, value); }
            }

            public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
                "RowSpacing", typeof(double), typeof(TablePanel), new PropertyMetadata(default(double), InRowSpacingUpdated));

            private static void InRowSpacingUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                TablePanel t = (TablePanel)d;
                t.InvalidateMeasure();
            }

            public double RowSpacing
            {
                get { return (double)GetValue(RowSpacingProperty); }
                set { SetValue(RowSpacingProperty, value); }
            }
            private List<Rect> _rects = new List<Rect>(20);
            private Size _cellAvailableSize;

            public static readonly DependencyProperty NumColumnsProperty = DependencyProperty.Register(
                "NumColumns", typeof(int), typeof(TablePanel), new PropertyMetadata(2, OnNumColumnsUpdated));

            private static void OnNumColumnsUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                TablePanel t = (TablePanel)d;
                t.InvalidateMeasure();
            }

            public int NumColumns
            {
                get { return (int)GetValue(NumColumnsProperty); }
                set { SetValue(NumColumnsProperty, value); }
            }
            public TablePanel()
            {

            }

            List<double> rowHeights = new List<double>(25);
            private List<double> columnWidths = new List<double>(3);

            public override void OnApplyTemplate()
            {

            }

            protected override Size MeasureOverride(Size availableSize)
            {
                var internalChildren = InternalChildren;
                var length = internalChildren.Count;
                if (_rects.Count < length)
                {
                    _rects.AddRange(Enumerable.Repeat(new Rect(), length - _rects.Count));
                }
                var rows = length / NumColumns + (length % NumColumns == 0 ? 0 : 1);
                var rowHeightsCount = rows - rowHeights.Count;
                if (rowHeightsCount > 0)
                    rowHeights.AddRange(Enumerable.Repeat(0.0, rowHeightsCount));
                var columnWidthsCount = NumColumns - columnWidths.Count;
                if (columnWidthsCount > 0)
                    columnWidths.AddRange(Enumerable.Repeat(0.0, columnWidthsCount));
                int col = 0;
                int row = 0;
                double rowHeight = 0.0;
                ;
                double totalHeight = 0.0;
                double Xpos = 0.0;
                double Ypos = 0.0;
                var i = 0;
                var avail = new Size(availableSize.Width - ColumnSpacing * NumColumns, availableSize.Height - RowSpacing * rows);
                _cellAvailableSize = new Size(avail.Width / NumColumns, avail.Height / rows);
                foreach (UIElement internalChild in internalChildren)
                {
                    internalChild.Measure(new Size(_cellAvailableSize.Width, Double.MaxValue));
                    var d1 = internalChild.DesiredSize;
                    internalChild.Measure(_cellAvailableSize);

                    rowHeight = Math.Max(Math.Max(rowHeight, internalChild.DesiredSize.Height), d1.Height);
                    if (internalChild.DesiredSize.Width > columnWidths[col])
                    {
                        columnWidths[col] = internalChild.DesiredSize.Width;
                    }

                    // _rects[i] = new Rect(col == 0? 0:columnWidths[col - 1], Ypos, columnWidths[col], rowHeight);
                    col = (col + 1) % NumColumns;
                    if (col == 0)
                    {
                        totalHeight += rowHeight;
                        Ypos += rowHeight + RowSpacing;
                        rowHeights[row] = rowHeight;
                        row++;
                    }

                    i++;
                }

                if (col != 0)
                {
                    totalHeight += rowHeight;
                    Ypos += rowHeight + RowSpacing;
                    rowHeights[row] = rowHeight;
                    row++;
                }

                var width = columnWidths.Sum() + NumColumns * ColumnSpacing;
                return new Size(width, Ypos);
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                var internalChildren = InternalChildren;
                var length = internalChildren.Count;
                var rows = length / NumColumns + (length % NumColumns == 0 ? 0 : 1);

                int col = 0;
                int row = 0;
                double rowHeight = 0.0;
                var totalHEight = rowHeights.Sum() + RowSpacing * rows;
                double excessPerRow = 0.0;
                if (totalHEight < finalSize.Height)
                {
                    excessPerRow = (finalSize.Height - totalHEight) / rows;
                    totalHEight = finalSize.Height;
                }

                double totalWidth = columnWidths.Sum() + ColumnSpacing * 2;
                IEnumerable<double> excessWidthPerCol = Enumerable.Repeat(0.0, columnWidths.Count);
                if (totalWidth < finalSize.Width)
                {
                    var excessWidth = finalSize.Width - totalWidth;
                    excessWidthPerCol = columnWidths.Select(c => c / totalWidth * excessWidth);
                    totalWidth = finalSize.Width;
                }
                double Xpos = 0.0;
                double Ypos = 0.0;
                var i = 0;
                var x = excessWidthPerCol.ToArray();
                foreach (UIElement internalChild in internalChildren)
                {
                    double width = columnWidths[col] + x[col];
                    double height = rowHeights[row] + excessPerRow;
                    var finalRect = new Rect(Xpos, Ypos, width, height);
                    internalChild.Arrange(finalRect);
                    // DebugUtils.WriteLine(finalRect.ToString());

                    col = (col + 1) % NumColumns;
                    if (col == 0)
                    {
                        row++;
                        Ypos += height + RowSpacing;
                        Xpos = 0;
                    }
                    else
                    {
                        Xpos += width + ColumnSpacing;
                    }

                    i++;
                }
                if (col != 0)
                {

                    Ypos += rowHeights[row] + excessPerRow + RowSpacing;
                    row++;
                    Xpos = 0;
                }

                return new Size(totalWidth, Ypos);
            }



        }
    }

