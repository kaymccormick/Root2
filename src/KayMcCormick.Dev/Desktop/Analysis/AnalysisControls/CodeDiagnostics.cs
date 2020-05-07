﻿using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    ///     <MyNamespace:CodeDiagnostics/>
    ///
    /// </summary>
    public class CodeDiagnostics : SyntaxNodeControl
    {
        private ListView _regions;
        private ListView _lines;
        private DrawingGroup _dgroup;
        private Rectangle _rect;

        static CodeDiagnostics()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CodeDiagnostics), new FrameworkPropertyMetadata(typeof(CodeDiagnostics)));
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            _regions = (ListView)GetTemplateChild("regions");
            if (_regions != null) _regions.SelectionChanged += RegionsOnSelectionChanged;
            _lines = (ListView)GetTemplateChild("lines");
            _dgroup = (DrawingGroup) GetTemplateChild("dgroup");
            _rect = (Rectangle) GetTemplateChild("rect");
            _lines.SelectionChanged += LinesOnSelectionChanged;
        }

        private void LinesOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LineInfo line = (LineInfo)_lines.SelectedItem;
            _dgroup.Children.Clear();
            
            _dgroup.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black, 1), new RectangleGeometry(new Rect(line.Origin.X, line.Origin.Y, line.Size.Width, line.Size.Height))));
            _rect.InvalidateVisual();
        }

        private void RegionsOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_regions.SelectedItem == null)
            {
                return;
            }

            LineInfo line = (LineInfo) _lines.SelectedItem;
            RegionInfo r = (RegionInfo) _regions.SelectedItem;
            Rect newR = new Rect(r.BoundingRect.X, 0, r.BoundingRect.Width, r.BoundingRect.Height);
            _dgroup.Children.Clear();
            _dgroup.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black, 1),
                new RectangleGeometry(new Rect(0, 0, line.Size.Width, line.Size.Height))));
            _dgroup.Children.Add(new GeometryDrawing(null, null, new RectangleGeometry(new Rect(0, 0, 10, 10))));
            _dgroup.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black, 1), new RectangleGeometry(newR)));
            foreach (var c in r.Characters)
            {
                _dgroup.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Red, 1),
                    new RectangleGeometry(new Rect(c.Bounds.X, c.Bounds.Y - r.BoundingRect.Y, c.Bounds.Width,
                        c.Bounds.Height))));
            }

            SaveImage(r);

            _rect.InvalidateVisual();
            
        }

        private void SaveImage(RegionInfo r)
        {
            DrawingVisual v = new DrawingVisual();
            var dc = v.RenderOpen();
            dc.DrawRectangle((DrawingBrush) GetTemplateChild("DrawingBrush"), null, _dgroup.Bounds);
            dc.Close();
            RenderTargetBitmap rtb = new RenderTargetBitmap((int) _dgroup.Bounds.Width, (int) _dgroup.Bounds.Height, 96,
                96,
                PixelFormats.Pbgra32);
            rtb.Render(v);
            ;
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            var fname = $"img_{_lines.SelectedIndex}-{r.Offset}.png";
            using (var s = File.Create("C:\\temp\\" + fname))
            {
                png.Save(s);
            }
        }
    }
}