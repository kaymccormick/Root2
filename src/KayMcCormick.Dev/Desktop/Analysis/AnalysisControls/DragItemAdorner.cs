using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using JetBrains.Annotations;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class DragItemAdorner : Adorner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adornedElement"></param>
        public DragItemAdorner([NotNull] UIElement adornedElement) : base(adornedElement)
        {
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, AdornedElement, DragDropEffects.All);
            }
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            var layer = (AdornerLayer)VisualTreeHelper.GetParent(this);

            Rect adornedElementRect = new Rect(0, 0, layer.ActualWidth, layer.ActualHeight);
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            double renderRadius = 5.0;
            drawingContext.DrawEllipse(renderBrush, renderPen,
                new Point(adornedElementRect.Width - 20, adornedElementRect.Height - 20), 10, 10);

        }
    }
}