using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AnalysisControls.RibbonModel;
using CsvHelper.Configuration;
using KayMcCormick.Dev;
using NLog;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonGroup : RibbonGroup
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private bool _dragging;
        private Vector _originalPosition;
        private Point _dragStartPosition;
        private Popup _dragPopup;
        private double _popupOriginalHorizOffset;
        private AdornerLayer _layer;
        private DragAdorner _dragAdorner;

        /// <inheritdoc />
        static MyRibbonGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonGroup),
                new FrameworkPropertyMetadata(typeof(MyRibbonGroup)));

        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            RibbonDebugUtils.OnPropertyChanged(this.ToString(), this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!e.Handled)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (!_dragging)
                    {
                        _dragging = true;
                        //CaptureMouse();
                        _dragStartPosition = e.GetPosition(this);
                        _originalPosition = VisualOffset;
                        _layer = AdornerLayer.GetAdornerLayer(ParentItemsControl);
                        _dragAdorner = new DragAdorner(ParentItemsControl, this);
                        _layer.Add(_dragAdorner);
                        // VisualBrush v = new VisualBrush(this);
                        // if (_dragPopup != null)
                        // {
                            // _dragPopup.IsOpen = false;
                        // }
                        // _dragPopup = new Popup();
                        // _dragPopup.Child = new Rectangle
                        // {
                            // Fill = v, Width = ActualWidth,
                            // Height = ActualHeight
                        // };
                        // _dragPopup.Placement = PlacementMode.MousePoint;
                        // _dragPopup.IsOpen = true;
                        // _popupOriginalHorizOffset  = _dragPopup.HorizontalOffset;
                    } else
                    {
                        DebugUtils.WriteLine("here");
                        var cur = e.GetPosition(this);
                        cur.Offset(-1 * _dragStartPosition.X, -1 * _dragStartPosition.GetHashCode());
                        if (_dragAdorner != null)
                        {
                            _dragAdorner.offsetX = cur.X;
                            _dragAdorner.offsetY = cur.Y;
                            _dragAdorner.InvalidateVisual();    
                            // _dragPopup.HorizontalOffset = _popupOriginalHorizOffset + cur.X;
                        }
                    }

                    var data = new DataObject();
                    var item
                        = ParentItemsControl.ItemContainerGenerator.ItemFromContainer(this);
                    if (item is RibbonModelGroup i)
                    {
                        StringWriter sw = new StringWriter();
                        var n = new CsvHelper.CsvWriter(sw, new CsvConfiguration(){Delimiter = "\t"});
                        foreach (var ribbonModelItem in i.Items)
                        {

                            n.WriteRecord(ribbonModelItem.GetType(), ribbonModelItem);
                        }
                        
                        var csv = sw.ToString();
                        data.SetData(DataFormats.Text, csv);
                    }
                    data.SetData("ModelObject", item);
                        
                    data.SetData(typeof(RibbonGroup), this);

                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move | DragDropEffects.Copy);
                }
            }
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            base.OnQueryContinueDrag(e);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                if (object.ReferenceEquals(e.Data.GetData(GetType()), this))
                {
                    e.Effects = DragDropEffects.Move;
                }
            }

        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonControl();
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            if (_dragging && _dragAdorner != null)
            {
                var cur = InputManager.Current.PrimaryMouseDevice.GetPosition(this);
                cur.Offset(-1 * _dragStartPosition.X, -1 * _dragStartPosition.GetHashCode());
                if (_dragAdorner != null)
                {
                    DebugUtils.WriteLine(cur.ToString());
                    _dragAdorner.offsetX = cur.X;
                    _dragAdorner.offsetY = cur.Y;
                    
                    // _dragPopup.HorizontalOffset = _popupOriginalHorizOffset + cur.X;
                }

            }
        }

        ItemsControl ParentItemsControl
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this);
            }
        }

    }

    public class DragAdorner : Adorner
    {
        private BitmapCacheBrush _brush;
        private Rect _rect;

        public DragAdorner(ItemsControl parentItemsControl, MyRibbonGroup myRibbonGroup):base(parentItemsControl)
        {
            _rect = new Rect(myRibbonGroup.DesiredSize);
            VisualBrush b = new VisualBrush(myRibbonGroup);
            BitmapCacheBrush z = new BitmapCacheBrush(myRibbonGroup);
            _brush = z;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
            var x = _rect;
            _rect.Offset(offsetX, offsetY);
            drawingContext.DrawRectangle(_brush, null, _rect);
        }

        public double offsetX { get; set; }

        public double offsetY { get; set; }
    }
}