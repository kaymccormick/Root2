﻿using System.ComponentModel ;
using System.Diagnostics ;
using System.Runtime.CompilerServices ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;
using AnalysisAppLib.ViewModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls.Views
{
    /// <summary>
    ///     Interaction logic for Types.xaml
    /// </summary>
    [ TitleMetadata ( "Syntax node types" ) ]
    public sealed partial class TypesView : UserControl
      , IView < ITypesViewModel >
      , IViewWithTitle
      , IControlView
      , INotifyPropertyChanged
    {
        private ITypesViewModel _viewModel ;

        /// <summary>
        /// </summary>
        public TypesView ( ) { InitializeComponent ( ) ; }

        /// <summary>
        /// </summary>
        /// <param name="viewModel"></param>
        public TypesView ( ITypesViewModel viewModel )
        {
            _viewModel = viewModel ;
            InitializeComponent ( ) ;
        }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        #region Implementation of IView<ITypesViewModel>
        /// <summary>
        /// </summary>
        public ITypesViewModel ViewModel
        {
            get { return _viewModel ; }
            set
            {
                _viewModel = value ;
                Debug.WriteLine ( "Set viewModel" ) ;
                OnPropertyChanged ( ) ;
            }
        }
        #endregion

        #region Implementation of IView1
        /// <summary>
        /// </summary>
        [ NotNull ] public string ViewTitle { get { return "Types View" ; } }
        #endregion

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Overrides of FrameworkElement
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate ( )
        {
            base.OnApplyTemplate ( ) ;
            DumpVisualRects ( this ) ;
        }

        private void DumpVisualRects ( [ NotNull ] DependencyObject reference )
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount ( reference ) ;
            for ( var i = 0 ; i < childrenCount ; i ++ )
            {
                var dependencyObject = VisualTreeHelper.GetChild ( reference , i ) ;
                var v = ( Visual ) dependencyObject ;
                var contentBounds = VisualTreeHelper.GetContentBounds ( v ) ;
                Debug.WriteLine (
                                 $"{v}{i} {contentBounds.Left},{contentBounds.Top} - {contentBounds.Right},{contentBounds.Bottom}"
                                ) ;
                DumpVisualRects ( v ) ;
            }
        }
        #endregion
    }
}