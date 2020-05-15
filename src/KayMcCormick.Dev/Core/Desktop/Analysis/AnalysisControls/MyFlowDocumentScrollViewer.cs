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
using JetBrains.Annotations ;
using NLog ;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MyFlowDocumentScrollViewer : FlowDocumentScrollViewer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>
        ///     Backing store for the <see cref="ScrollViewer" /> property.
        /// </summary>
        private ScrollViewer _scrollViewer ;

        /// <summary>
        /// 
        /// </summary>
        public bool DoOverrideMeasure { get ; set ; }

        /// <summary>
        ///     Gets the scroll viewer contained within the FlowDocumentScrollViewer
        ///     control
        /// </summary>
        [ CanBeNull ] public ScrollViewer ScrollViewer
        {
            get
            {
                if ( _scrollViewer != null )
                {
                    return _scrollViewer ;
                }

                DependencyObject obj = this ;

                do
                {
                    if ( VisualTreeHelper.GetChildrenCount ( obj ) > 0 )
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        obj = VisualTreeHelper.GetChild ( obj as Visual , 0 ) ;
                    }
                    else
                    {
                        return null ;
                    }
                }
                while ( ! ( obj is ScrollViewer ) ) ;

                _scrollViewer = obj as ScrollViewer ;

                return _scrollViewer ;
            }
        }

        #region Overrides of Visual
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visualAdded"></param>
        /// <param name="visualRemoved"></param>
        protected override void OnVisualChildrenChanged (
            DependencyObject visualAdded
          , DependencyObject visualRemoved
        )
        {
            Logger.Debug ( "{added}" , visualAdded ) ;
            if ( visualAdded is Visual v )
            {
                var pp = v.PointToScreen ( new Point ( 0 , 0 ) ) ;
                Logger.Debug ( "point is {pp}" , pp ) ;
            }

            base.OnVisualChildrenChanged ( visualAdded , visualRemoved ) ;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride ( Size availableSize )
        {
            if ( ! DoOverrideMeasure )
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