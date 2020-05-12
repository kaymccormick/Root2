using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using KayMcCormick.Dev;

namespace KayMcCormick.Lib.Wpf
{
    public class TablePanel : Panel
    {
        public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
            "ColumnSpacing", typeof(double), typeof(TablePanel), new PropertyMetadata(default(double)));

        public double ColumnSpacing
        {
            get { return (double) GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
            "RowSpacing", typeof(double), typeof(TablePanel), new PropertyMetadata(default(double)));

        public double RowSpacing
        {
            get { return (double) GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }
        private List<Rect> _rects = new List<Rect>(20);
        private Size _cellAvailableSize;

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
            if (_rects.Count < internalChildren.Count)
            {
                _rects.AddRange(Enumerable.Repeat(new Rect(), internalChildren.Count - _rects.Count));
            }
            var rows = internalChildren.Count / 2;
            rowHeights.AddRange(Enumerable.Repeat(0.0, rows - rowHeights.Count));
            

            var X = new[] {0.0, 0.0};
            int col = 0;
            int row = 0;
            double rowHeight = 0.0;
            ;
            double totalHeight = 0.0;
            double Xpos = 0.0;
            double Ypos = 0.0;
            var i = 0;
            var avail = new Size(availableSize.Width - ColumnSpacing * 2, availableSize.Height - RowSpacing * rows);
            _cellAvailableSize = new Size(avail.Width / 2, avail.Height / rows);
            foreach (UIElement internalChild in internalChildren)
            {
                internalChild.Measure(new Size(_cellAvailableSize.Width, Double.MaxValue));
                var d1 = internalChild.DesiredSize;
                internalChild.Measure(_cellAvailableSize);

                rowHeight = Math.Max(Math.Max(rowHeight, internalChild.DesiredSize.Height), d1.Height);
                if (internalChild.DesiredSize.Width > X[col])
                {
                    X[col] = internalChild.DesiredSize.Width;
                }

                _rects[i] = new Rect(col == 0? 0:X[0], Ypos, X[col], rowHeight);
                col = (col + 1) % 2;
                if (col == 0)
                {
                    
                    totalHeight += rowHeight;
                    Ypos += rowHeight + RowSpacing;
                    rowHeights[row] = rowHeight;
                    row++;
                }

                i++;
            }
            columnWidths.Clear();
            columnWidths.AddRange(X);

            return new Size(X[0] + X[1] +ColumnSpacing * 2, Ypos);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var internalChildren = InternalChildren;
            var rows = internalChildren.Count / 2;
            
            var X = new[] { 0.0, 0.0 };
            int col = 0;
            int row = 0;
            double rowHeight = 0.0;
            var totalHEight = rowHeights.Sum() + RowSpacing * rows;
            double excessPerRow = 0.0;
            if (totalHEight < finalSize.Height)
            {
                excessPerRow = (finalSize.Height - totalHEight) / rows;
            }

            double totalWidth = columnWidths.Sum() + ColumnSpacing * 2;
            IEnumerable<double> excessWidthPerCol = Enumerable.Repeat(0.0, columnWidths.Count);
            if(totalWidth < finalSize.Height)
            {
                var excessWidth = finalSize.Width - totalWidth;
                excessWidthPerCol = columnWidths.Select(c => c / totalWidth * excessWidth);
            }
            double Xpos = 0.0;
            double Ypos = 0.0;
            var i = 0;
            var x = excessWidthPerCol.ToArray();
            foreach (UIElement internalChild in internalChildren)
            {
                double width = columnWidths[col] + x[col] ;
                double height = rowHeights[row] + excessPerRow;  
                internalChild.Arrange(new Rect(Xpos, Ypos, width, height));

                //_rects[i] = new Rect(col == 0 ? 0 : X[0], Ypos, X[col], rowHeight);
                col = (col + 1) % 2;
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


            return new Size(totalWidth, Ypos);
        }

    }
}
