﻿using System.Windows.Controls;
using System.Windows.Input;
using AnalysisControls.ViewModel;
using Autofac;
using JetBrains.Annotations ;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
#if PYTHON
    /// <summary>
    ///     Interaction logic for PythonControl.xaml
    /// </summary>
    [ TitleMetadata ( "Python" ) ]
    public sealed partial class PythonControl : UserControl
      , IView < PythonViewModel >
      , IView1
      , IControlView
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ILifetimeScope _scope ;

        /// <summary>
        /// 
        /// </summary>
        public PythonControl ( ) : this ( null , null ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="viewModel"></param>
        public PythonControl ( ILifetimeScope scope , PythonViewModel viewModel )
        {
            _scope    = scope ;
            ViewModel = viewModel ;
            InitializeComponent ( ) ;

            ViewModel.FlowDOcument = flow ;
        }

    #region Implementation of IView<out PythonViewModel>
        /// <summary>
        /// 
        /// </summary>
        public PythonViewModel ViewModel { get ; }
    #endregion

    // ReSharper disable once UnusedMember.Local
    private void UIElement_OnKeyDown ( object sender , [ NotNull ] KeyEventArgs e )
    {
        switch ( e.Key )
        {
            case Key.Enter :
            {
                DebugUtils.WriteLine ( "rceived key " + e.Key ) ;
                var textBox = ( TextBox ) sender ;
                ViewModel.TakeLine ( textBox.Text ) ;
                textBox.Text = "" ;
                break ;
            }
            case Key.Up : ViewModel.HistoryUp ( ) ;
                break ;
        }
    }

    // ReSharper disable once UnusedMember.Local
    // ReSharper disable once UnusedParameter.Local
    private void UIElement_OnPreviewKeyDown ( object sender , [ NotNull ] KeyEventArgs e )
        {
            DebugUtils.WriteLine ( "rceived key " + e.Key ) ;
            switch ( e.Key )
            {
                case Key.Up :
                    ViewModel.HistoryUp ( ) ;
                    e.Handled = true ;
                    break ;
                case Key.Down :
                    ViewModel.HistoryDown ( ) ;
                    e.Handled = true ;
                    break ;
            }
        }
    }
#endif
}