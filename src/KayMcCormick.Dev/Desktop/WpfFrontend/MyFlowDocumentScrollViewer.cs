#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// MyFlowDocumentScrollViewer.cs
// 
// 2020-03-02-2:46 PM
// 
// ---
#endregion
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;
using NLog ;

namespace AnalysisControls
{
    public class MyFlowDocumentScrollViewer : FlowDocumentScrollViewer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of Visual
        protected override void OnVisualChildrenChanged (
            DependencyObject visualAdded
          , DependencyObject visualRemoved
        )
        {
            Logger.Debug ( "{added}" , visualAdded ) ;
            if ( visualAdded is Visual v )
            {
                var pp = v.PointToScreen ( new Point ( 0 , 0 ) ) ;
                Logger.Debug ( "point is {pp}", pp ) ;
            }

            base.OnVisualChildrenChanged ( visualAdded , visualRemoved ) ;
        }
        #endregion
        #region Overrides of UIElement
        // ReSharper disable once RedundantOverriddenMember
        protected override void OnRender ( DrawingContext drawingContext )
        {
            base.OnRender ( drawingContext ) ;
            
        }
        #endregion

        /// <summary>
        /// Backing store for the <see cref="ScrollViewer"/> property.
        /// </summary>
        private ScrollViewer scrollViewer ;

        public bool doOverrideMeasure { get ; set ; }

        /// <summary>
        /// Gets the scroll viewer contained within the FlowDocumentScrollViewer control
        /// </summary>
        public ScrollViewer ScrollViewer
        {
            get
            {
                if ( scrollViewer == null )
                {
                    DependencyObject obj = this ;

                    do
                    {
                        if ( VisualTreeHelper.GetChildrenCount ( obj ) > 0 )
                        {
                            obj = VisualTreeHelper.GetChild ( obj as Visual , 0 ) ;
                        }
                        else
                        {
                            return null ;
                        }
                    }
                    while ( ! ( obj is ScrollViewer ) ) ;

                    scrollViewer = obj as ScrollViewer ;
                }

                return scrollViewer ;
            }
        }

        protected override Size MeasureOverride ( Size availableSize )
        {
            if ( ! doOverrideMeasure )
            {
                return base.MeasureOverride ( availableSize ) ;
            }

            var panelDesiredSize = new Size ( ) ;
            if ( double.IsInfinity ( availableSize.Height ) )
            {
                panelDesiredSize.Height = 924 ;
            }
            else
            {
                panelDesiredSize.Height = availableSize.Height - 100 ;
            }

            if ( double.IsInfinity ( panelDesiredSize.Width ) )
            {
                panelDesiredSize.Width = 1000 ;
            }
            else
            {
                panelDesiredSize.Width = availableSize.Width ;
            }

            return panelDesiredSize ;
        }
    }
}