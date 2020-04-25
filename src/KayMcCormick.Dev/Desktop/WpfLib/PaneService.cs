using System.Windows.Markup ;
using AvalonDock.Layout ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public sealed class PaneWrapper : IAddChild
    {
        private LayoutAnchorable _anchorable ;

        /// <summary>
        /// </summary>
        /// <param name="anchorable"></param>
        public PaneWrapper ( LayoutAnchorable anchorable ) { Anchorable = anchorable ; }

        /// <summary>
        /// </summary>
        public LayoutAnchorable Anchorable
        {
            get { return _anchorable ; }
            set { _anchorable = value ; }
        }

        #region Implementation of IAddChild
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        public void AddChild ( object value ) { Anchorable.Content = value ; }

        /// <summary>
        /// </summary>
        /// <param name="text"></param>
        public void AddText ( string text ) { }
        #endregion
    }

    /// <summary>
    /// </summary>
    public sealed class PaneService
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public PaneWrapper GetPane ( )
        {
//            LayoutAnchorablePane model = new LayoutAnchorablePane();
            var pane = new LayoutAnchorable ( ) ;
            return new PaneWrapper ( pane ) ;
        }
    }
}