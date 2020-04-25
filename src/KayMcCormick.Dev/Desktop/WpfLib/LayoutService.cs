#region header
// Kay McCormick (mccor)
// 
// Analysis
// WpfLib
// LayoutService.cs
// 
// 2020-04-24-2:58 PM
// 
// ---
#endregion
using AvalonDock.Layout ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public sealed class LayoutService
    {
        private readonly LayoutAnchorablePane _anchorablePane ;

        /// <summary>
        /// </summary>
        /// <param name="anchorablePane"></param>
        public LayoutService ( [ NotNull ] LayoutAnchorablePane anchorablePane )
        {
            _anchorablePane = anchorablePane ;
        }

        /// <summary>
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="makeActive"></param>
        // ReSharper disable once UnusedMember.Global
        public void AddToLayout ( [ NotNull ] PaneWrapper wrapper , bool makeActive = true )
        {
            _anchorablePane.Children.Add ( wrapper.Anchorable ) ;
            if ( makeActive )
            {
                _anchorablePane.SelectedContentIndex =
                    _anchorablePane.IndexOf ( wrapper.Anchorable ) ;
            }
        }
    }
}