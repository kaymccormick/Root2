using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup ;
using AvalonDock.Controls ;
using AvalonDock.Layout ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    public class PaneWrapper : IAddChild
    {
        private LayoutAnchorable _anchorable ;

        public PaneWrapper ( LayoutAnchorable anchorable ) { Anchorable = anchorable ; }

        public LayoutAnchorable Anchorable
        {
            get { return _anchorable ; }
            set { _anchorable = value ; }
        }

        #region Implementation of IAddChild
        public void AddChild ( object value )
        {
            Anchorable.Content = value ;
        }

        public void AddText ( string text ) { }
        #endregion
    }
    public class PaneService
    {
        public PaneWrapper GetPane ( )
        {
//            LayoutAnchorablePane model = new LayoutAnchorablePane();
            var pane = new LayoutAnchorable();
            return new PaneWrapper(pane);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LayoutService
    {
        private readonly LayoutAnchorablePane _anchorablePane ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anchorablePane"></param>
        public LayoutService ( [ NotNull ] LayoutAnchorablePane anchorablePane )
        {
            _anchorablePane = anchorablePane ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="makeActive"></param>
        public void AddToLayout ( PaneWrapper wrapper, bool makeActive = true )
        {
            _anchorablePane.Children.Add (wrapper.Anchorable  );
            if ( makeActive )
            {
                _anchorablePane.SelectedContentIndex =
                    _anchorablePane.IndexOf ( wrapper.Anchorable ) ;
            }
        }
    }
}
