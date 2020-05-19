using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KmDevWpfControls
{
    public class TablePanel5 : Panel
    {
        public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
            "ColumnSpacing", typeof(double), typeof(TablePanel5), new PropertyMetadata(default(double), OnColumnSpacingUpdated));

        private static void OnColumnSpacingUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TablePanel5 t = (TablePanel5)d;
            t.InvalidateMeasure();
        }

        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
            "RowSpacing", typeof(double), typeof(TablePanel5), new PropertyMetadata(default(double), InRowSpacingUpdated));

        private static void InRowSpacingUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TablePanel5 t = (TablePanel5)d;
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
            "NumColumns", typeof(int), typeof(TablePanel5), new PropertyMetadata(2, OnNumColumnsUpdated));

        private static void OnNumColumnsUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TablePanel5 t = (TablePanel5)d;
            t.InvalidateMeasure();
        }

        public int NumColumns
        {
            get { return (int)GetValue(NumColumnsProperty); }
            set { SetValue(NumColumnsProperty, value); }
        }
        public TablePanel5()
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