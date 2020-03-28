using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup ;
using AvalonDock.Controls ;
using AvalonDock.Layout ;

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
        public IAddChild GetPane ( )
        {
//            LayoutAnchorablePane model = new LayoutAnchorablePane();
            var pane = new LayoutAnchorable();
            return new PaneWrapper(pane);
        }
    }

    public class LayoutService
    {
        private LayoutAnchorablePane _anchorablePane ;

        public LayoutService ( LayoutAnchorablePane anchorablePane )
        {
            _anchorablePane = anchorablePane ;
        }

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
